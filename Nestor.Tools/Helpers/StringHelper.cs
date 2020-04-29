using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Nestor.Tools.Helpers
{
    public static class StringHelper
    {
        private static Regex wordRegex = new Regex(@"\p{Lu}\p{Ll}+|\p{Lu}+(?!\p{Ll})|\p{Ll}+|\d+");

        /// <summary>
        /// Remplace dans la chaine de caractère passée en entrée les caractères du tableau passé en entrée
        /// </summary>
        /// <param name="input">Chaine à nettoyer</param>
        /// <param name="charsToRemove">Tableau contenant les caractères à nettoyer</param>
        /// <returns></returns>
        public static string Remove(this string input, char[] charsToRemove)
        {
            string cleanString = input;

            foreach (var c in charsToRemove)
                cleanString = cleanString.Replace(c.ToString(), string.Empty);

            return cleanString;
        }

        /// <summary>
        /// Méthode qui indique si une valeur passée en paramètre et de type numérique
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string input)
        {
            return new Regex(@"^[-+]?\d*[.,]?\d*$").IsMatch(input);
        }

        /// <summary>
        /// Transforme une chaine de caractère passée en paramètre en chaine à la casse "Pascal"
        /// </summary>
        /// <param name="input">chaine de caractère</param>
        /// <returns></returns>
        public static string ToPascalCase(this string input)
        {
            return wordRegex.Replace(input, EvaluatePascal);
        }

        /// <summary>
        /// Transforme la chaine de caractères passée au format CamelCase
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string input)
        {
            string pascal = ToPascalCase(input);
            return wordRegex.Replace(pascal, EvaluateFirstCamel, 1);
        }

        private static string EvaluateFirstCamel(Match match)
        {
            return match.Value.ToLower();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private static string EvaluatePascal(Match match)
        {
            string value = match.Value;
            int valueLength = value.Length;

            if (valueLength == 1)
                return value.ToUpper();
            else
            {
                if (valueLength <= 2 && IsWordUpper(value))
                    return value;
                else
                    return value.Substring(0, 1).ToUpper() + value.Substring(1, valueLength - 1).ToLower();
            }
        }

        /// <summary>
        /// Indique si la chaine de caractère 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private static bool IsWordUpper(string word)
        {
            bool result = true;

            foreach (char c in word)
            {
                if (Char.IsLower(c))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Retire les espaces en trop d'une chaine de caratères 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveMultipleSpaces(this string input)
        {
            return RegexHelper.Replace(input, RegexHelper.RegexType.MultipleSpaces, " ");
        }

        public static string Titleize(this string text)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text).ToSentenceCase().ReplaceSpecialCharactersBySpace();
        }

        /// <summary>
        /// Remplace la première lettre d'un mot par une majuscule, le reste en minuscule
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToSentenceCase(this string str)
        {
            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
        }

        public static string ReplaceSpecialCharactersBySpace(this string str)
        {
            return Regex.Replace(str, @"\W+", " ");
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            return Regex.Replace(str, @"\W+", string.Empty);
        }


        public static string Take(this string str, int nbChar)
        {
            if (str.Length <= nbChar)
                return str;
            else
                return str.Substring(0, nbChar);
        }

        /// <summary>
        /// Remplace tous les caractères spéciaux par un tiret
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceSpecialCharactersByHyphen(this string str)
        {
            return Regex.Replace(str, @"\W+", "-");
        }

        public static string NormalizeStringForUrl(string name, char replace = '-')
        {
            String normalizedString = name.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder(name.Length);

            foreach (char c in normalizedString)
            {
                switch (CharUnicodeInfo.GetUnicodeCategory(c))
                {
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.DecimalDigitNumber:
                        stringBuilder.Append(c);
                        break;
                    case UnicodeCategory.SpaceSeparator:
                    case UnicodeCategory.ConnectorPunctuation:
                    case UnicodeCategory.DashPunctuation:
                        stringBuilder.Append(replace);
                        break;
                }
            }
            string result = stringBuilder.ToString();
            return String.Join(replace.ToString(), result.Split(new char[] { replace }
                , StringSplitOptions.RemoveEmptyEntries)).ToLower(); // remove duplicate dash
        }

        /// <summary>
        /// Méthode qui supprime les caractères accentués d'une chaine de caractères
        /// </summary>
        /// <param name="text">Entrée</param>
        /// <returns></returns>
        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Obtient une chaine de caractère aléatoire
        /// </summary>
        /// <param name="maxLength">Longueur maximale</param>
        /// <returns></returns>
        public static string GetRandomString(int maxLength)
        {
            return GenerateRandomString("ACDEFGHJKLMNPQRTUVWXY34679".ToCharArray(), maxLength);
        }



        /// <summary>
        /// Obtient une chaine de caractère aléatoire
        /// </summary>
        /// <param name="maxLength">Longueur maximale</param>
        /// <returns></returns>
        public static string GetRandomNumericString(int maxLength)
        {
            return GenerateRandomString("0123456789".ToCharArray(), maxLength);
        }

        /// <summary>
        /// Transforme la première lettre en majuscule
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FirstLetterToUpper(this string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        /// <summary>
        /// Génère une chaine aléatoire à partir d'un tableau de charactères et d'une longueur max
        /// </summary>
        /// <param name="availableChars">Tableau des caractères disponibles</param>
        /// <param name="maxLength">Longueur attendue de la chaine</param>
        /// <returns></returns>
        private static string GenerateRandomString(char[] availableChars, int maxLength)
        {
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxLength];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxLength);
            foreach (byte b in data)
            {
                result.Append(availableChars[b % (availableChars.Length)]);
            }
            return result.ToString();
        }

        /// <summary>
        /// Converti une chaine du type 12312312312345 au format siret 123 123 123 12345
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string ToSiretNumber(this string helper)
        {
            string siret;
            try
            {
                siret = Regex.Replace(helper, "(?<first>([0-9]{3}))(?<second>([0-9]{3}))(?<third>([0-9]{3}))(?<fourth>([0-9]{5}))",
                    "${first} ${second} ${third} ${fourth}");
            }
            catch (ArgumentException)
            {
                siret = helper;
            }
            return siret;
        }

        /// <summary>
        /// Retourne une chaîne contenant un nombre spécifié de caractères en partant de la gauche d'une chaîne.
        /// </summary>
        /// <param name="s">chaine dont les caractères situés le plus à gauche sont retournés</param>
        /// <param name="count">Nombre de caractères à retourner</param>
        /// <returns>une chaîne contenant le nombre spécifié de caractères en partant de la gauche de s</returns>
        public static string Left(this string s, int count)
        {
            //s.CheckArgumentNull("s");
            //count.CheckArgumentOutOfRange("count", 0, s.Length, ExceptionMessage.SubstringCountOutOfRange);
            return s.Substring(0, count);
        }

        /// <summary>
        /// Retourne une chaîne contenant un nombre spécifié de caractères en partant de la droite d'une chaîne.
        /// </summary>
        /// <param name="s">chaine dont les caractères situés le plus à droite sont retournés</param>
        /// <param name="count">Nombre de caractères à retourner</param>
        /// <returns>une chaîne contenant le nombre spécifié de caractères en partant de la droite de s</returns>
        public static string Right(this string s, int count)
        {
            //s.CheckArgumentNull("s");
            //count.CheckArgumentOutOfRange("count", 0, s.Length, ExceptionMessage.SubstringCountOutOfRange);
            return s.Substring(s.Length - count, count);
        }

        /// <summary>
        /// Converti une chaine sécurisé en chaine non sécurisée
        /// </summary>
        /// <param name="secureString"></param>
        /// <returns></returns>
        public static string ConvertToUnSecureString(this SecureString secureString)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        /// <summary>
        /// Converti une chaine de caractère en chaine sécurisée
        /// </summary>
        /// <param name="strPassword"></param>
        /// <returns></returns>
        public static SecureString convertToSecureString(this string strPassword)
        {
            var secureStr = new SecureString();
            if (strPassword.Length > 0)
            {
                foreach (var c in strPassword.ToCharArray()) secureStr.AppendChar(c);
            }
            return secureStr;
        }

        public static bool IsNotEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}
