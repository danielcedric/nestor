using System.Text.RegularExpressions;

namespace Nestor.Tools.Helpers
{
    public class PhoneHelper
    {
        /// <summary>
        ///     Méthode qui tente d'internationaliser un numéro de téléphone mobile fourni au format 'France métropolitaine'
        /// </summary>
        /// <param name="input">Numéro de téléphone en entrée</param>
        /// <param name="internationalizedMobilePhone">Numéro de téléphone converti au format international</param>
        /// <returns>Vrai si la conversion a fonctionné</returns>
        public static bool TryInternationalizeMobilePhone(string input, out string internationalizedMobilePhone)
        {
            // Nettoyage de la chaine de caractère
            var cleanInput = input.Replace(" ", string.Empty).Trim();

            // On teste si l'entrée n'est pas déjà au format internationnal
            if (cleanInput.IsMatch(RegexHelper.RegexType.FrenchInternationalMobilePhone))
            {
                internationalizedMobilePhone = cleanInput;
                return true;
            }

            if (cleanInput.IsMatch(RegexHelper.RegexType.FrenchMobilePhone))
            {
                // L'entrée est au format "France métropolitaine", on va le convertir
                // En enlevant le premier 0.
                cleanInput = cleanInput.Substring(1);
                cleanInput = string.Format("+33{0}", cleanInput);

                if (cleanInput.IsMatch(RegexHelper.RegexType.FrenchInternationalMobilePhone))
                {
                    internationalizedMobilePhone = cleanInput;
                    return true;
                }
            }

            internationalizedMobilePhone = null;
            return false;
        }

        /// <summary>
        ///     Méthode qui tente d'internationaliser un numéro de téléphone fourni au format 'France métropolitaine'
        /// </summary>
        /// <remarks>Fonctionne aussi pour les numéros de mobile</remarks>
        /// <param name="input">Numéro de téléphone en entrée</param>
        /// <param name="internationalizedPhone">Numéro de téléphone converti au format international</param>
        /// <returns>Vrai si la conversion a fonctionné</returns>
        public static bool TryInternationalizeFrenchPhone(string input, out string internationalizedPhone)
        {
            // Nettoyage de la chaine de caractère
            var cleanInput = CleanPhone(input).Trim();

            // On teste si l'entrée n'est pas déjà au format internationnal
            if (cleanInput.IsMatch(RegexHelper.RegexType.FrenchInternationalPhone))
            {
                internationalizedPhone = cleanInput;
                return true;
            }

            if (cleanInput.IsMatch(RegexHelper.RegexType.FrenchPhone))
            {
                // L'entrée est au format "France métropolitaine", on va le convertir
                // En enlevant le premier 0.
                cleanInput = cleanInput.Substring(1);
                cleanInput = string.Format("+33{0}", cleanInput);

                if (cleanInput.IsMatch(RegexHelper.RegexType.FrenchInternationalPhone))
                {
                    internationalizedPhone = cleanInput;
                    return true;
                }
            }

            internationalizedPhone = null;
            return false;
        }

        /// <summary>
        ///     retourne un booléen qui indique si le numéro de téléphone est au format mobile Français
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsFrenchMobilePhone(string input)
        {
            return input.IsMatch(RegexHelper.RegexType.FrenchMobilePhone);
        }

        /// <summary>
        ///     Retourne un booléen qui indique si le numéro de téléphone est au format Français.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsFrenchPhone(string input)
        {
            return input.IsMatch(RegexHelper.RegexType.FrenchPhone);
        }

        /// <summary>
        ///     retourne un booléen qui indique si le numéro de téléphone est au format mobile international Français (+33)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsFrenchInternationalMobilePhone(string input)
        {
            return input.IsMatch(RegexHelper.RegexType.FrenchInternationalMobilePhone);
        }

        /// <summary>
        ///     retourne un booléen qui indique si le numéro de téléphone est au format international Français (+33)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsFrenchInternationalPhone(string input)
        {
            return input.IsMatch(RegexHelper.RegexType.FrenchInternationalPhone);
        }

        /// <summary>
        ///     Méthode qui nettoie le numéro de téléphone passé en paramètre (suppression des ' ', '-', ...)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CleanPhone(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            return new Regex(@"[^\d]").Replace(input, string.Empty);
        }

        public static string ToFrenchPhone(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            input = CleanPhone(input.Replace(" ", string.Empty));

            if (input.Length == 10 && input[0] == '0')
                return input;
            if (input.Length == 9)
                return string.Format("0{0}", input);

            if (input.StartsWith("+33"))
                return input.Replace("+33", "0");

            if (input.StartsWith("33"))
                return string.Format("0{0}", input.Substring(2, input.Length - 2));

            return input;
        }

        /// <summary>
        ///     Formate un numéro de téléphone selon le séparateur fourni
        /// </summary>
        /// <param name="input">Numéro de téléphone à mettre en forme</param>
        /// <param name="separator">séparateur, par défaut l'espace</param>
        /// <returns></returns>
        public static string HumanizePhone(string input, string separator = " ")
        {
            if (string.IsNullOrEmpty(input))
                return null;

            var regex = new Regex(@"(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})");
            return regex.Replace(input, string.Format("$1{0}$2{0}$3{0}$4{0}$5", separator));
        }
    }
}