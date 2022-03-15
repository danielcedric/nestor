using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nestor.Tools.Infrastructure.EntityFramework.Exceptions
{
    public class DbEntityValidationException : Exception
    {
        public IEnumerable<ValidationResult> EntityValidationErrors { get; private set; }

        public DbEntityValidationException(System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult> validationResults)
        {
            this.EntityValidationErrors = validationResults;
        }
    }
}
