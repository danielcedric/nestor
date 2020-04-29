using Nestor.Tools.Domain.DataAnnotations;
using Nestor.Tools.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nestor.Tools.Domain.Helpers;
using System.Linq;

namespace Nestor.Tools.Domain.Abstractions
{
    public abstract class Entity : IEntity, IEquatable<IEntity>, IComparable<IEntity>
    {

        #region Properties
        /// <summary>
        /// Affecte ou obtient un dictionnaire contenant les propriétés qui ont changées
        /// Key = nom de la propriété
        /// Value = Tuple[AncienneValeur, NouvelleValeur]
        /// </summary>
        protected virtual Dictionary<string, Tuple<dynamic, dynamic>> ValuesChanged { get; set; } = new Dictionary<string, Tuple<dynamic, dynamic>>();

        #endregion

        #region Methods
        /// <summary>
        /// recopie l'ensemble des propriétés simples (membres par valeur).
        /// ne recopie par les champs qui composent la clé unique ainsi que l'identifiant.
        /// </summary>
        /// <param name="other">objet à recopier</param>
        /// <returns>copie</returns>
        public void CopyFrom(IEntity other)
        {
            this.ShallowCopy(other);
        }

        /// <summary>
        /// Méthode de validation de l'objet
        /// Déclenche une exception avec les erreurs recontrées
        /// </summary>
        public void ValidateObject()
        {
            this.ValidateObject();
        }

        /// <summary>
        /// Méthode de validation de l'objet
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ValidationResult> TryValidateObject()
        {
            return this.TryValidateObject();
        }

        /// <summary>
        /// Sert de fonction de hachage pour l'objet en cours.
        /// </summary>
        /// <returns>Code de hachage pour l'objet en cours.</returns>
        public override int GetHashCode()
        {
            int? hashCode = HashCodeHelper.GetHashCode(this);
            return hashCode.HasValue ? hashCode.Value : base.GetHashCode();
        }

        /// <summary>
        /// Détermine si l'objet spécifié est identique à l'objet actuel.
        /// </summary>
        /// <param name="obj">Objet à comparer avec l'objet actif. </param>
        /// <returns>true si l'objet spécifié est égal à l'objet actif ; sinon, false.</returns>
        public override bool Equals(object obj)
        {
            return GetHashCode().Equals(obj.GetHashCode());
        }

        /// <summary>
        /// Compare l'objet en cours à un autre objet du même type.
        /// </summary>
        /// <param name="other">Objet à comparer avec cet objet.</param>
        /// <returns>Valeur qui indique l'ordre relatif des objets comparés.</returns>
        public virtual int CompareTo(IEntity other)
        {
            return GetHashCode().CompareTo(other.GetHashCode());
        }

        /// <summary>
        /// Indique si l'objet actuel est égal à un autre objet du même type.
        /// </summary>
        /// <param name="other">Objet à comparer avec cet objet.</param>
        /// <returns>true si l'objet en cours est égal au paramètre other ; sinon, false.</returns>
        public virtual bool Equals(IEntity other)
        {
            return Equals(other as object);
        }

        /// <summary>
        /// Surcharge de la méthode ToString afin d'afficher les valeurs des propriétés uniques
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            unchecked
            {
                try
                {
                    var entityType = this.GetType();
                    var uniqueByAttribute = entityType.GetCustomAttributes(typeof(UniqueByAttribute), true).FirstOrDefault() as UniqueByAttribute;
                    if (uniqueByAttribute != null)
                    {
                        var properties = entityType.GetProperties().Where(prop => uniqueByAttribute.PropertyNames.Contains(prop.Name)).OrderBy(prop => prop.Name);
                        IList<object> values = new List<object>();

                        foreach (var property in properties)
                            values.Add($"{property.Name}={property.GetValue(this)}");

                        return values.Any() ? string.Join(", ", values) : base.ToString();
                    }

                    return base.ToString();
                }
                catch (Exception e)
                {
                    return $"{e.Message}: {base.ToString()}";
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// Entité générique qui sera mappée dans un ORM
    /// Ce type sera à utiliser pour les classes qui ont une clé primaire composée
    /// </summary>
    public abstract class EntityWithCompositeId : Entity, IEntityWithCompositeId
    {

    }

    /// <summary>
    /// Entité générique qui sera mappée dans un ORM
    /// </summary>
    /// <typeparam name="T">Type de l'identifiant</typeparam>
    public abstract class EntityWithId<T> : Entity, IEntityWithId<T>
    {
        #region Properties
        /// <summary>
        /// Affecte ou obtient l'identifiant de type <typeparamref name="T"/>
        /// </summary>
        [Id]
        public T Id { get; set; }

        /// <summary>
        /// Obtient si l'instance actuelle est transiante
        /// </summary>
        public bool IsTransient { get { return (default(T)).Equals(this.Id); } }

        #endregion

        #region Constructors    
        public EntityWithId()
        {

        }
        #endregion


    }

    /// <summary>
    /// Entité de base qui sera mappée dans un ORM.
    /// L'id sera de type Int32
    /// </summary>
    public abstract class EntityWithId : EntityWithId<Guid>, IEntityWithId<Guid>
    {

    }

    /// <summary>
    /// Type d'entité avec des données concernant le suivi 
    /// </summary>
    public abstract class EntityWithTracking : EntityWithId, IEntityWithId
    {
        #region Properties
        /// <summary>
        /// Affecte ou obtient la date de création de l'objet
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Affecte ou obtient le nom d'utilisateur de la personne qui a créé l'objet
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Affecte ou obtient la date de dernière mise à jour de l'objet
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        /// <summary>
        /// Affecte ou obtient le nom d'utilisateur de la personne qui a modifié l'objet
        /// </summary>
        public string UpdatedBy { get; set; }
        /// <summary>
        /// Affecte ou obtient la date de suppression de l'objet
        /// </summary>
        public DateTime? DeletedAt { get; set; }
        /// <summary>
        /// Affecte ou obtient le nom d'utilisateur de la personne qui a supprimé l'objet
        /// </summary>
        public string DeletedBy { get; set; }
        /// <summary>
        /// Obtient un booléen qui indique si l'édition a été supprimée
        /// </summary>
        public bool HasBeenDeleted { get { return DeletedAt.HasValue; } }
        #endregion

        #region Constructors
        public EntityWithTracking() : base()
        {
            this.CreatedAt = DateTime.Now;
        }
        #endregion
    }
}
