using System.Collections.Generic;
using System.Linq;
using Nestor.Tools.Domain.Entities;
using Nestor.Tools.Domain.DataAnnotations;

namespace Nestor.Tools.Domain.Helpers
{
    /// <summary>
    ///     Helper qui permet de mutualiser la logique de génération d'un HashCode
    /// </summary>
    public static class HashCodeHelper
    {
        /// <summary>
        ///     31 est une valeur communément utilisée en informatique pour tout ce qui concerne la génération de clés de hashage.
        ///     Il s'agirait d'un des nombre premier entier qui donne le moins de collisions lors de la génération des clés.
        ///     https://computinglife.wordpress.com/2008/11/20/why-do-hash-functions-use-prime-numbers/
        /// </summary>
        private static readonly int primeNumber = 31;

        /// <summary>
        ///     Obtient le HashCode correspondant aux parametres donnés en entrée. L'ordre des champs n'a pas d'importance
        /// </summary>
        /// <remarks>
        ///     Le HashCode est une valeur qui est utilisée dans la comparaison de 2 entités. Il est utilisé dans les méthodes
        ///     Equals() et CompareTo().
        ///     Ne pas oublier de surcharger et d'implémenter les méthodes GetHashCode(), Equals(obj), CompareTo(obj)
        /// </remarks>
        /// <param name="input">Propriétés servant à définir le hashcode</param>
        /// <returns>Valeur du hashcode calculé</returns>
        public static int GetHashCode(params object[] input)
        {
            unchecked
            {
                var hash = 0;
                for (var i = 0; i < input.Length; i++)
                    hash ^= primeNumber * (input[i] != null ? input[i].GetHashCode() : 0);
                return hash;
            }
        }

        /// <summary>
        ///     Obtient le HashCode d'une entité en se basant sur la présence des attributs <see cref="UniqueKeyAttribute" /> ou
        ///     <see cref="UniqueByAttribute" />
        /// </summary>
        /// <remarks>
        ///     Le HashCode est une valeur qui est utilisée dans la comparaison de 2 entités. Il est utilisé dans les méthodes
        ///     Equals() et CompareTo().
        ///     Ne pas oublier de surcharger et d'implémenter les méthodes GetHashCode(), Equals(obj), CompareTo(obj)
        /// </remarks>
        /// <param name="entity">Entité à évaluer</param>
        /// <returns>Valeur du hashcode calculé</returns>
        public static int? GetHashCode(IEntity entity)
        {
            var hashCode = EvaluateHashCodeWithUniqueOnClass(entity);
            return hashCode.HasValue ? hashCode.Value : EvaluateHashCodeWithUniqueOnProperties(entity);
        }

        /// <summary>
        ///     Evalue le hashcode de l'objet en sa basant sur l'attribut <see cref="UniqueByAttribute" /> indiqué sur la classe
        /// </summary>
        /// <param name="entity">Objet à évaluer</param>
        /// <returns></returns>
        private static int? EvaluateHashCodeWithUniqueOnClass(IEntity entity)
        {
            var entityType = entity.GetType();
            var uniqueByAttribute =
                entityType.GetCustomAttributes(typeof(UniqueByAttribute), true).FirstOrDefault() as UniqueByAttribute;
            if (uniqueByAttribute != null)
            {
                var properties = entityType.GetProperties()
                    .Where(prop => uniqueByAttribute.PropertyNames.Contains(prop.Name)).OrderBy(prop => prop.Name);
                IList<object> values = new List<object>();

                foreach (var property in properties) values.Add(property.GetValue(entity));

                return values.Any() ? GetHashCode(values.ToArray()) : (int?) null;
            }

            return null;
        }

        /// <summary>
        ///     Evalue le hashcode de l'objet en sa basant sur les attributs <see cref="UniqueKeyAttribute" /> indiqués sur les
        ///     propriétés
        /// </summary>
        /// <param name="entity">Objet à évaluer</param>
        /// <returns></returns>
        private static int? EvaluateHashCodeWithUniqueOnProperties(IEntity entity)
        {
            var properties = entity.GetType().GetProperties().OrderBy(prop => prop.Name);
            IList<object> values = new List<object>();

            foreach (var property in properties)
            {
                var uniqueKeyAttribute =
                    property.GetCustomAttributes(typeof(UniqueKeyAttribute), true).FirstOrDefault() as
                        UniqueKeyAttribute;
                if (uniqueKeyAttribute != null)
                    values.Add(property.GetValue(entity));
            }

            return values.Any() ? GetHashCode(values.ToArray()) : (int?) null;
        }
    }
}