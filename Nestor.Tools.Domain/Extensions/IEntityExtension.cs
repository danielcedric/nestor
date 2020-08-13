using System;
using System.Collections.Generic;
using Nestor.Tools.Domain.Abstractions;
using Nestor.Tools.Domain.DataAnnotations;
using Nestor.Tools.Helpers;

namespace Nestor.Tools.Domain.Extensions
{
    public static class IEntityExtension
    {
        #region Properties

        /// <summary>
        ///     Affecte ou obtient un dictionnaire contenant les propriétés qui ont changées
        ///     Key = nom de la propriété
        ///     Value = Tuple[AncienneValeur, NouvelleValeur]
        /// </summary>
        private static Dictionary<string, Tuple<dynamic, dynamic>> ValuesChanged { get; } =
            new Dictionary<string, Tuple<dynamic, dynamic>>();

        #endregion

        #region Methods

        /// Recopie l'ensemble des propriétés simples (membres par valeur).
        /// Ne recopie par les champs qui composent la clé unique ainsi que l'identifiant.
        /// </summary>
        /// <param name="entity">Entité ou copier les valeurs</param>
        /// <param name="other">Entité depuis laquelle on va copier les valeurs</param>
        public static void ShallowCopy(this IEntity entity, IEntity other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            if (entity.GetType() != other.GetType())
                throw new InvalidOperationException(
                    "La recopie des propriétés d'un objet nécessite que le type source et le type cible soient identiques.");

            // Récupération des propriétés de l'objet à recopier. Les attributs ne doivent pas être notés comme Id ou UniqueKey
            //foreach (var property in AttributeHelper.GetValueAndPropertyNameWithAttributeExceptions(other, new Type[] { typeof(IdAttribute), typeof(UniqueKeyAttribute) }, true))
            foreach (var property in AttributeHelper.GetValueAndPropertyNameWithAttributeExceptions(other,
                new[] {typeof(IdAttribute)}, true))
            {
                var propertyToSetValue = entity.GetType().GetProperty(property.Key);

                // Si la propriété n'est pas nulle et que l'on est sur une propriété dont la valeur n'est pas une référence.
                if (propertyToSetValue != null && PrimitiveTypesHelper.IsByValueType(propertyToSetValue.PropertyType) &&
                    propertyToSetValue.SetMethod != null)
                    propertyToSetValue.SetValue(entity, property.Value);

                // Détection des changements de propriétés sur le model afin de logguer les évènements de modification de valeurs
                var oldValue = propertyToSetValue.GetValue(entity);
                if (oldValue != property.Value)
                {
                    if (!ValuesChanged.ContainsKey(propertyToSetValue.Name))
                        ValuesChanged.Add(propertyToSetValue.Name,
                            new Tuple<dynamic, dynamic>(oldValue, property.Value));
                    else
                        ValuesChanged[propertyToSetValue.Name] = new Tuple<dynamic, dynamic>(oldValue, property.Value);
                }
            }
        }

        #endregion
    }
}