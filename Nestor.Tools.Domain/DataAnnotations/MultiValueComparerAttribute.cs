using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Nestor.Tools.Domain.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class MultiValueComparerAttribute : ValidationAttribute
    {
        #region Constructors

        /// <summary>
        ///     Constructeur par défaut.
        /// </summary>
        /// <param name="valueToCompare">Valeur à comparer</param>
        /// <param name="expectedValuesByProperties">
        ///     Dictionnaire qui contient des association clé / valeur avec en clé le nom de
        ///     la propriété à comparer et en valeur, la valeur attendue.
        /// </param>
        public MultiValueComparerAttribute(object valueToCompare, params object[] expectedValuesByProperties)
        {
            if (!expectedValuesByProperties.Any() || expectedValuesByProperties.Count() % 2 == 1)
                throw new ArgumentException("La liste des propriétés à valider est vide.");

            ValueToCompare = valueToCompare;
            ExpectedValuesByProperties = new Dictionary<string, object>();
            for (var i = 0; i < expectedValuesByProperties.Length / 2; i++)
                ExpectedValuesByProperties.Add((string) expectedValuesByProperties[2 * i],
                    expectedValuesByProperties[2 * i + 1]);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Détermine si les valeurs des propriétés spécifiées sont valides. Les types des propriétés doivent être du même
        ///     type.
        /// </summary>
        /// <param name="value">La valeur de l'objet à valider</param>
        /// <param name="validationContext">Le contexte de validation</param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Si la valeur en cours de la propriété n'est pas celle attendue, alors on arrête la validation
            if (!Equals(value, ValueToCompare))
                return null;

            // Comparaison des types des propriétés à comparer
            var typeToCompare = validationContext.ObjectType.GetProperty(ExpectedValuesByProperties.First().Key)
                .PropertyType;
            foreach (var otherProperty in ExpectedValuesByProperties)
            {
                // La propriété à valider.
                var property = validationContext.ObjectType.GetProperty(otherProperty.Key);

                if (property == null)
                    return new ValidationResult(string.Format("Propriété inconnue : {0}.", otherProperty.Key));

                // Comparaison des types
                if (!property.PropertyType.Equals(typeToCompare))
                    return new ValidationResult(
                        "Les types de toutes les propriétés à comparer doivent-être identiques.");

                // Comparaison des valeurs
                var other = property.GetValue(validationContext.ObjectInstance, null);

                if (!Equals(other, otherProperty.Value))
                    return new ValidationResult("La condition de validité n'est pas remplie.");
            }

            // Toutes les valeurs sont cohérentes, alors on retourne null.
            return null;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Affecte ou obtient la valeur qui déclenche la validation
        /// </summary>
        public object ValueToCompare { get; set; }

        /// <summary>
        ///     Affecte ou obtient un dictionnaire qui contient pour chacune des propriétés à comparer, la valeur attendue
        /// </summary>
        public IDictionary<string, object> ExpectedValuesByProperties { get; set; }

        #endregion
    }
}