using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Nestor.Tools.Domain.DataAnnotations;
using Nestor.Tools.Domain.Helpers;

namespace Nestor.Tools.Domain.Entities
{
    public abstract class Entity : IEntity, IEquatable<IEntity>, IComparable<IEntity>, INotifyPropertyChanged
    {
        #region Properties
        
        /// <summary>
        /// Déclenché lorsqu'une propriété de la classe a été modifiée
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Obtient un booléen qui indique si l'objet a été modifié depuis sa création
        /// </summary>
        public bool IsDirty { get; private set; }
        #endregion

        #region Methods

        /// <summary>
        ///     recopie l'ensemble des propriétés simples (membres par valeur).
        ///     ne recopie par les champs qui composent la clé unique ainsi que l'identifiant.
        /// </summary>
        /// <param name="other">objet à recopier</param>
        /// <returns>copie</returns>
        public void CopyFrom(IEntity other)
        {
            this.ShallowCopy(other);
        }

        /// <summary>
        ///     Méthode de validation de l'objet
        ///     Déclenche une exception avec les erreurs recontrées
        /// </summary>
        public void ValidateObject()
        {
            ValidateObject();
        }

        /// <summary>
        ///     Méthode de validation de l'objet
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ValidationResult> TryValidateObject()
        {
            return TryValidateObject();
        }

        /// <summary>
        ///     Sert de fonction de hachage pour l'objet en cours.
        /// </summary>
        /// <returns>Code de hachage pour l'objet en cours.</returns>
        public override int GetHashCode()
        {
            var hashCode = HashCodeHelper.GetHashCode(this);
            return hashCode.HasValue ? hashCode.Value : base.GetHashCode();
        }

        /// <summary>
        ///     Détermine si l'objet spécifié est identique à l'objet actuel.
        /// </summary>
        /// <param name="obj">Objet à comparer avec l'objet actif. </param>
        /// <returns>true si l'objet spécifié est égal à l'objet actif ; sinon, false.</returns>
        public override bool Equals(object obj)
        {
            return GetHashCode().Equals(obj.GetHashCode());
        }

        /// <summary>
        ///     Compare l'objet en cours à un autre objet du même type.
        /// </summary>
        /// <param name="other">Objet à comparer avec cet objet.</param>
        /// <returns>Valeur qui indique l'ordre relatif des objets comparés.</returns>
        public virtual int CompareTo(IEntity other)
        {
            return GetHashCode().CompareTo(other.GetHashCode());
        }

        /// <summary>
        ///     Indique si l'objet actuel est égal à un autre objet du même type.
        /// </summary>
        /// <param name="other">Objet à comparer avec cet objet.</param>
        /// <returns>true si l'objet en cours est égal au paramètre other ; sinon, false.</returns>
        public virtual bool Equals(IEntity other)
        {
            return Equals(other as object);
        }

        /// <summary>
        ///     Surcharge de la méthode ToString afin d'afficher les valeurs des propriétés uniques
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            try
            {
                var entityType = GetType();
                var uniqueByAttribute =
                    entityType.GetCustomAttributes(typeof(UniqueByAttribute), true).FirstOrDefault() as
                        UniqueByAttribute;
                if (uniqueByAttribute != null)
                {
                    var properties = entityType.GetProperties()
                        .Where(prop => uniqueByAttribute.PropertyNames.Contains(prop.Name)).OrderBy(prop => prop.Name);
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

        /// <summary>
        /// Affecte une nouvelle valeur à la propriété passée en paramètre et déclenche l'événement PropertyChanged
        /// </summary>
        /// <typeparam name="T">Type du paramètre à setter</typeparam>
        /// <param name="name">Nom de la propriété à setter</param>
        /// <param name="oldValue">Ancienne valeur</param>
        /// <param name="newValue">Nouvelle valeur</param>
        /// <returns>Vrai si la valeur à été mise à jour, faux sinon</returns>
        protected bool SetProperty<T>(string name, ref T oldValue, T newValue) where T : System.IComparable<T>
        {
            if (oldValue == null || oldValue.CompareTo(newValue) != 0)
            {
                oldValue = newValue;
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(name));
                IsDirty = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Affecte une nouvelle valeur à la propriété nommée en paramètre et déclenche l'événement PropertyChanged.
        /// A utiliser pour les types nullables
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="name">nom de la propriété à setter</param>
        /// <param name="oldValue">Ancienne valeur</param>
        /// <param name="newValue">Nouvelle valeur</param>
        /// <returns>Vrai si la valeur à été mise à jour, faux sinon</returns>
        protected void SetProperty<T>(string name, ref Nullable<T> oldValue, Nullable<T> newValue) where T : struct, System.IComparable<T>
        {
            if (oldValue.HasValue != newValue.HasValue || (newValue.HasValue && oldValue.Value.CompareTo(newValue.Value) != 0))
            {
                oldValue = newValue;
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(name));
                IsDirty = true;
            }
        }
        #endregion
    }

    /// <summary>
    ///     Entité générique qui sera mappée dans un ORM
    ///     Ce type sera à utiliser pour les classes qui ont une clé primaire composée
    /// </summary>
    public abstract class EntityWithComposedPrimaryKey : Entity, IEntityWithComposedPrimaryKey
    {
        public EntityWithComposedPrimaryKey()
        {
            
        }
    }

    /// <summary>
    ///     Entité générique qui sera mappée dans un ORM
    /// </summary>
    /// <typeparam name="T">Type de l'identifiant</typeparam>
    public abstract class Entity<T> : Entity, IEntity<T>
    {
        #region Constructors
        public Entity()
        {
        }
        #endregion

        #region Properties

        /// <summary>
        ///     Affecte ou obtient l'identifiant de type <typeparamref name="T" />
        /// </summary>
        [Id]
        public T Id { get; set; }

        /// <summary>
        ///     Obtient si l'instance actuelle est transiante
        /// </summary>
        public bool IsTransient => default(T).Equals(Id);
        #endregion
    }
}