using System;
using System.ComponentModel.DataAnnotations;
using Nestor.Tools.Algorithms;

namespace Nestor.Tools.Domain.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class SiretAttribute : ValidationAttribute
    {
        #region Methods

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return null;

            if (Siret.Check(value.ToString()))
                return null;
            return new ValidationResult("Le numéro de SIRET n'est pas valide.");
        }

        #endregion
    }
}