using System;
using System.ComponentModel.DataAnnotations;

namespace Nestor.Tools.Domain.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MinAttribute : ValidationAttribute
    {
        #region Properties
        /// <summary>
        /// Affecte ou obtient la valeur minimale
        /// </summary>
        public double MinValue { get; set; }
        #endregion

        #region Constructors
        public MinAttribute(double min)
        {
            this.MinValue = min;
        }
        #endregion

        #region Methods
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value.GetType() != typeof(double))
                return new ValidationResult("La valeur doit-être de type double");

            if (((double)value) < MinValue)
                return new ValidationResult(string.Format("La valeur de {0} doit-être supérieure à {1}", validationContext.DisplayName, MinValue));

            return null;
        }
        #endregion
    }
}
