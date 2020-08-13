using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Nestor.Tools.Helpers
{
    public static class NumberHelper
    {
        public static bool IsInteger(string value)
        {
            return value.IsMatch(RegexHelper.RegexType.IsInteger);
        }

        public static int ParseInt32(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            var tabVal = value.Replace(',', '.').Split('.'); // cas de 124.00 par exemple
            return int.Parse(tabVal[0], CultureInfo.InvariantCulture); // cas de 124.00 par exemple
        }

        public static short ParseInt16(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            var tabVal = value.Replace(',', '.').Split('.'); // cas de 124.00 par exemple
            return short.Parse(tabVal[0], CultureInfo.InvariantCulture);
        }

        public static double ParseDouble(string value)
        {
            return double.Parse(!string.IsNullOrEmpty(value) ? value.Replace(',', '.') : value,
                CultureInfo.InvariantCulture);
        }

        public static bool TryParseDouble(string value, out double result)
        {
            return double.TryParse(!string.IsNullOrEmpty(value) ? value.Replace(',', '.') : value,
                NumberStyles.AllowDecimalPoint ^ NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture,
                out result);
        }

        public static decimal ParseDecimal(string value)
        {
            return decimal.Parse(!string.IsNullOrEmpty(value) ? value.Replace(',', '.') : value,
                CultureInfo.InvariantCulture);
        }

        public static bool TryParseDecimal(string value, out decimal result)
        {
            return decimal.TryParse(!string.IsNullOrEmpty(value) ? value.Replace(',', '.') : value,
                NumberStyles.AllowDecimalPoint ^ NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture,
                out result);
        }

        public static string Format(double valeur)
        {
            return Convert.ToString(valeur, CultureInfo.InvariantCulture);
        }

        public static string Format(double valeur, string format)
        {
            return string.Format(format, valeur, CultureInfo.InvariantCulture);
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
        ///     Converti un nombre de bytes passés en paramètre en unité lisible pour l'Homme
        /// </summary>
        /// <param name="byteLength"></param>
        /// <returns></returns>
        public static string ByteLengthToHumanString(long byteLength)
        {
            string[] suf = {"o", "Ko", "Mo", "Go", "To", "Po", "Eo"}; //Longs run out around EB
            if (byteLength == 0)
                return "0" + suf[0];
            var bytes = Math.Abs(byteLength);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return string.Format("{0} {1}", (Math.Sign(byteLength) * num).ToString(), suf[place]);
        }

        /// <summary>
        ///     récupère un double placé en début de string, exemple : "12.85 Litres"  retourne  "12.85"
        /// </summary>
        /// <param name="measure"></param>
        /// <returns>true si la convertion a réussi</returns>
        public static bool TryMeasureToDouble(string measure, out double result)
        {
            var test = false; //la variable que l'on va retourner
            result = 0;

            //^[0-9] on récupère d'abord les premiers numeriques
            //[.,] ensuite les ., 
            //[0-9] et numériques encore
            var regex = new Regex(@"^[0-9]([.,][0-9])?$");

            //on cherche les occurences qui correspondent
            var match = regex.Match(measure);

            //s'il y'a des occurences on parse pour avoir le résultat en double, en utilisant tryparse pas de souci de convertion
            if (match.Captures.Count > 0) test = double.TryParse(match.Captures[0].Value, out result);

            return test;
        }

        /// <summary>
        ///     Converti un string en double
        /// </summary>
        /// <param name="text"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryConvertStringToDouble(string text, out double result)
        {
            var separator = ",";

            text = Regex.Replace(text, @"[\s]+", ""); // Supprime tous les espaces

            var m = Regex.Match(text,
                "(?<separator>)[^0-9]+[0-9]*$"); // Cherche la dernière suite de caractère non numérique de la chaine
            if (m != null && m.Success)
                separator = m.Groups["separator"].Value; // S'en sert comme séparateur

            // Créer un formateur de nombre avec ce séparateur
            var nfi = new NumberFormatInfo();
            nfi.CurrencyDecimalSeparator = separator;
            nfi.NumberDecimalSeparator = separator;
            nfi.PercentDecimalSeparator = separator;

            // Tente de convertir le nombre avec ce format
            return double.TryParse(text, NumberStyles.Any, nfi, out result);
        }

        /// <summary>
        ///     Convert to radians
        /// </summary>
        /// <param name="valInDegree">The value to convert to radians</param>
        /// <returns>The value in radians</returns>
        public static double ToRadians(this double valInDegree)
        {
            return Math.PI / 180 * valInDegree;
        }
    }
}