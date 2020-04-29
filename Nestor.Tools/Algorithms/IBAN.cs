using Nestor.Tools.Helpers;
using System;
using System.Text;

namespace Nestor.Tools.Algorithms
{
    /// <summary>
    /// Vérification d'un numéro IBAN
    /// </summary>
    public static class IBAN
    {
        /// <summary>
        /// Vérifie la validité d'un numéro IBAN
        /// </summary>
        /// <param name="number">Numéro IBAN à vérifier</param>
        /// <returns>true si le numéro est valide, false sinon</returns>
        public static bool Check(string number)
        {
            var clean = BankingCommon.CleanInput(number);
            var rotated = clean.Substring(4) + clean.Substring(0, 4);
            var onlyDigits = ReplaceLetters(rotated);
            var modulo = MathHelper.Modulo(onlyDigits, 97);
            return modulo == 1;
        }

        /// <summary>
        /// Obtient un numéro IBAN à partir d'un numéro RIB et d'un code de pays
        /// </summary>
        /// <param name="rib">Le numéro RIB</param>
        /// <param name="countryCode">Le code du pays (par exemple "FR" pour la France)</param>
        /// <returns>Le numéro IBAN correspondant au RIB et au code pays spécifiés</returns>
        public static string FromRIB(string rib, string countryCode)
        {
            var clean = BankingCommon.CleanInput(rib);
            var onlyDigits = ReplaceLetters(clean + countryCode + "00");
            var modulo = MathHelper.Modulo(onlyDigits, 97);
            return string.Format("{0}{1:00}{2}", countryCode, 98 - modulo, clean);
        }

        private static string ReplaceLetters(string input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in input)
            {
                if (Char.IsDigit(c))
                {
                    sb.Append(c);
                }
                else // lettre majuscule
                {
                    int n = c - 'A' + 10;
                    sb.Append(n);
                }
            }
            return sb.ToString();
        }
    }
}
