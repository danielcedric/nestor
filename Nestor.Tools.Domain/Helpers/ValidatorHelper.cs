using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nestor.Tools.Domain.Helpers
{
    public static class ValidatorHelper
    {
        public static void ValidateObject(this object obj)
        {
            var context = new ValidationContext(obj, null, null);
            var result =
                obj.TryValidateObject(); //System.ComponentModel.DataAnnotations.Validator.ValidateObject(obj, context, true);
            if (result != null)
            {
                var errors = new StringBuilder();
                foreach (var e in result)
                    errors.AppendLine(string.Format("- {0}", e.ErrorMessage));

                throw new ValidationException(errors.ToString());
            }
        }

        /// <summary>
        ///     Valide l'objet passé en paramètre
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ICollection<ValidationResult> TryValidateObject(this object obj)
        {
            var context = new ValidationContext(obj, null, null);
            var result = new List<ValidationResult>();

            if (Validator.TryValidateObject(obj, context, result, true))
                return null;
            return result;
        }
    }
}