using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Nestor.Tools.Helpers;

namespace Nestor.Tools.Domain.DataAnnotations
{
    /// <summary>
    /// Enum qui indique un critère de comparaison entre 2 valeurs
    /// </summary>
    public enum Comparator
    {
        [Description("=")]
        EqualTo,
        [Description("!=")]
        NotEqualTo,
        [Description(">")]
        GreaterThan,
        [Description("<")]
        LessThan,
        [Description(">=")]
        GreatThanOrEqualTo,
        [Description("<=")]
        LessThanOrEqualTo
    }

    /// <summary>
    /// http://cncrrnt.com/blog/index.php/2011/01/custom-validationattribute-for-comparing-properties/
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CompareValuesAttribute : ValidationAttribute
    {
        #region Properties
        /// <summary>
        /// Affecte ou obtient l'autre propriété à comparer
        /// </summary>
        public string OtherProperty { get; set; }
        /// <summary>
        /// Affecte ou obtient le comparateur à appliquer
        /// </summary>
        public Comparator Comparator { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="otherProperty">Autre propriété à comparer</param>
        /// <param name="comparator">Comparateur à appliquer</param>
        public CompareValuesAttribute(string otherProperty, Comparator comparator)
        {
            if (otherProperty == null)
                throw new ArgumentNullException("OtherProperty");

            OtherProperty = otherProperty;
            Comparator = comparator;
        }
        #endregion

        /// <summary>
        /// Détermine si la valeur spécifiée de l'objet est valide. Pour que cela soit le cas, les objets doivent être du même type.
        /// La présence d'une valeur NULL retourne false, sauf si les 2 valeurs sont nulles.
        /// Les objets devront mettre en oeuvre IComparable pour les instances GreaterThan, LessThan, GreatThanOrEqualTo et LessThanOrEqualTo
        /// </summary>
        /// <param name="value">La valeur de l'objet à valider</param>
        /// <param name="validationContext">Le contexte de validation</param>
        /// <returns>Un résultat de validation si l'objet est invalide, NULL si la validation est valide</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // La propriété à valider.
            var property = validationContext.ObjectType.GetProperty(OtherProperty);

            if (property == null)
                return new ValidationResult(String.Format("Propriété inconnue : {0}.", OtherProperty));

            // Comparaison des types
            if (validationContext.ObjectType.GetProperty(validationContext.MemberName).PropertyType != property.PropertyType)
                return new ValidationResult(String.Format("Les types de {0} et {1} doivent-être identiques.", validationContext.DisplayName, OtherProperty));

            // Récupération de l'autre valeur
            var other = property.GetValue(validationContext.ObjectInstance, null);

            // Comparaison
            if (Comparator == Comparator.EqualTo)
            {
                if (Object.Equals(value, other))
                    return null;
            }
            else if (Comparator == Comparator.NotEqualTo)
            {
                if (!Object.Equals(value, other))
                    return null;
            }
            else
            {
                // Vérification que les 2 objets à comparer implémentent IComparable
                //if (!(value is IComparable) || !(other is IComparable))
                //    return new ValidationResult(String.Format("{0} et {1} doivent implémenter tous les deux IComparable", validationContext.DisplayName, OtherProperty));

                // Comparaison des objets
                var result = Comparer<IComparable>.Default.Compare((IComparable)value, (IComparable)other);

                switch (Comparator)
                {
                    case Comparator.GreaterThan:
                        if (result > 0)
                            return null;
                        break;
                    case Comparator.LessThan:
                        if (result < 0)
                            return null;
                        break;
                    case Comparator.GreatThanOrEqualTo:
                        if (result >= 0)
                            return null;
                        break;
                    case Comparator.LessThanOrEqualTo:
                        if (result <= 0)
                            return null;
                        break;
                }
            }

            // got this far must mean the items don't meet the comparison criteria
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        /// <summary>
        /// Applique le formattage du message d'erreur
        /// </summary>
        /// <param name="name">The name to include in the error message</param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, base.ErrorMessageString, name, OtherProperty, Comparator.ToDescription());
        }

        /// <summary>
        /// retrieve the object to compare to
        /// </summary>
        /// <returns></returns>
        private object GetOther(ValidationContext context)
        {
            return null;
        }
    }
}
