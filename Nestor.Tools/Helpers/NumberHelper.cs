using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Nestor.Tools.Helpers
{
    public static class NumberHelper
    {
        public static bool IsInteger(string value)
        {
            return RegexHelper.IsMatch(value, RegexHelper.RegexType.IsInteger);
        }

        public static Int32 ParseInt32(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            string[] tabVal = value.Replace(',', '.').Split('.'); // cas de 124.00 par exemple
            return Int32.Parse(tabVal[0], CultureInfo.InvariantCulture); // cas de 124.00 par exemple
        }

        public static Int16 ParseInt16(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            string[] tabVal = value.Replace(',', '.').Split('.'); // cas de 124.00 par exemple
            return Int16.Parse(tabVal[0], CultureInfo.InvariantCulture);
        }

        public static Double ParseDouble(string value)
        {
            return Double.Parse(!string.IsNullOrEmpty(value) ? value.Replace(',', '.') : value, CultureInfo.InvariantCulture);
        }

        public static bool TryParseDouble(string value, out double result)
        {
            return Double.TryParse(!string.IsNullOrEmpty(value) ? value.Replace(',', '.') : value, NumberStyles.AllowDecimalPoint ^ NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);
        }

        public static Decimal ParseDecimal(string value)
        {
            return Decimal.Parse(!string.IsNullOrEmpty(value) ? value.Replace(',', '.') : value, CultureInfo.InvariantCulture);
        }

        public static bool TryParseDecimal(string value, out decimal result)
        {
            return Decimal.TryParse(!string.IsNullOrEmpty(value) ? value.Replace(',', '.') : value, NumberStyles.AllowDecimalPoint ^ NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);
        }

        public static string Format(double valeur)
        {
            return Convert.ToString(valeur, CultureInfo.InvariantCulture);
        }

        public static string Format(double valeur, string format)
        {
            return String.Format(format, valeur, CultureInfo.InvariantCulture);
        }

        public static string Format(int valeur)
        {
            return Convert.ToString(valeur, CultureInfo.InvariantCulture);
        }

        public static string Format(short valeur)
        {
            return Convert.ToString(valeur, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converti un nombre de bytes passés en paramètre en unité lisible pour l'Homme
        /// </summary>
        /// <param name="byteLength"></param>
        /// <returns></returns>
        public static String ByteLengthToHumanString(long byteLength)
        {
            string[] suf = { "o", "Ko", "Mo", "Go", "To", "Po", "Eo" }; //Longs run out around EB
            if (byteLength == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteLength);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return String.Format("{0} {1}", (Math.Sign(byteLength) * num).ToString(), suf[place]);
        }

        /// <summary>
        /// récupère un double placé en début de string, exemple : "12.85 Litres"  retourne  "12.85"
        /// </summary>
        /// <param name="measure"></param>
        /// <returns>true si la convertion a réussi</returns>
        public static bool TryMeasureToDouble(string measure, out double result)
        {
            bool test = false;//la variable que l'on va retourner
            result = 0;

            //^[0-9] on récupère d'abord les premiers numeriques
            //[.,] ensuite les ., 
            //[0-9] et numériques encore
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^[0-9]([.,][0-9])?$");

            //on cherche les occurences qui correspondent
            System.Text.RegularExpressions.Match match = regex.Match(measure);

            //s'il y'a des occurences on parse pour avoir le résultat en double, en utilisant tryparse pas de souci de convertion
            if (match.Captures.Count > 0)
            {
                test = Double.TryParse(match.Captures[0].Value, out result);
            }

            return test;
        }

        /// <summary>
        /// Converti un string en double
        /// </summary>
        /// <param name="text"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryConvertStringToDouble(string text, out double result)
        {
            string separator = ",";

            text = Regex.Replace(text, @"[\s]+", ""); // Supprime tous les espaces

            Match m = Regex.Match(text, "(?<separator>)[^0-9]+[0-9]*$"); // Cherche la dernière suite de caractère non numérique de la chaine
            if (m != null && m.Success)
                separator = m.Groups["separator"].Value; // S'en sert comme séparateur

            // Créer un formateur de nombre avec ce séparateur
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.CurrencyDecimalSeparator = separator;
            nfi.NumberDecimalSeparator = separator;
            nfi.PercentDecimalSeparator = separator;

            // Tente de convertir le nombre avec ce format
            return double.TryParse(text, NumberStyles.Any, nfi, out result);
        }

        /// <summary>
        /// Convert to radians
        /// </summary>
        /// <param name="valInDegree">The value to convert to radians</param>
        /// <returns>The value in radians</returns>
        public static double ToRadians(this double valInDegree)
        {
            return (Math.PI / 180) * valInDegree;
        }
    }
}
