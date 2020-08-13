using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Nestor.Tools.Helpers
{
    /// <summary>
    ///     Idées de Regex : https://manual.limesurvey.org/Using_regular_expressions
    /// </summary>
    public static class RegexHelper
    {
        public enum RegexType
        {
            [Description(@"(?<=\s|^)\d+(?=\s|$)")] IsInteger,
            [Description("^(0|[1-9][0-9]*)$")] IsNumeric,

            [Description("((0[1-9])|([1-8][0-9])|(9[0-8])|(2A)|(2B))[0-9]{3}")]
            FrenchZipCode,

            [Description(@"[^()[\]{}*&^%$#@!0-9]+")]
            CityName,
            [Description(@"\s+")] MultipleSpaces,
            [Description(@"(BP)\s?(\d+)")] FrenchPostBox,
            [Description(@"(CEDEX)\s?(\d+)")] FrenchCedex,

            [Description(
                @"^([a-zA-ZáàâäãåçéèêëíìîïñóòôöõúùûüýÿæœÁÀÂÄÃÅÇÉÈÊËÍÌÎÏÑÓÒÔÖÕÚÙÛÜÝŸÆŒ._\s-]*)\ \(((0[1-9])|([1-8][0-9])|(9[0-8])|(2A)|(2B))[0-9]{3}\)")]
            CityFullNameWithFrenchZipCode,
            [Description(@"\{(.*?)\}")] RefServiceLeafCategoryNameWithAlias,

            [Description(@"^0[679](\s?\d{2}){4}$")]
            FrenchMobilePhone,

            [Description(@"^((\+|00)33\s?)[679](\s?\d{2}){4}$")]
            FrenchInternationalMobilePhone,

            [Description(@"^0[1-9](\s?\d{2}){4}$")]
            FrenchPhone,

            [Description(@"^((\+|00)33\s?)[1-9](\s?\d{2}){4}$")]
            FrenchInternationalPhone
        }

        /// <summary>
        ///     Indique si la chaine de caractère passée en paramètre match avec le type de regex choisi
        /// </summary>
        /// <param name="regexType"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMatch(this string input, RegexType regexType)
        {
            return new Regex(regexType.ToDescription()).IsMatch(input);
        }

        /// <summary>
        ///     Recherche dans la chaine spécifiée en paramètre la première occurence trouvée qui correspond au type de regex
        ///     choisi
        /// </summary>
        /// <param name="regexType"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Match Match(this string input, RegexType regexType)
        {
            return new Regex(regexType.ToDescription()).Match(input);
        }

        /// <summary>
        ///     Recherche dans la chaine spécifiée en paramètre toutes les occurences qui correspondent au type de regex choisi
        /// </summary>
        /// <param name="regexType"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static MatchCollection Matches(this string input, RegexType regexType)
        {
            return new Regex(regexType.ToDescription()).Matches(input);
        }

        /// <summary>
        ///     Supprime les matches de la chaine de caractères passée en paramètre.
        /// </summary>
        /// <param name="regexType">type de regex</param>
        /// <param name="input">chaine à nettoyer</param>
        /// <param name="trim">Si vrai, nettoie le résultat des espaces générés</param>
        /// <returns></returns>
        public static string Remove(this string input, RegexType regexType, bool trim)
        {
            if (trim)
                return new Regex(regexType.ToDescription()).Replace(input, string.Empty).Trim();
            return new Regex(regexType.ToDescription()).Replace(input, string.Empty);
        }

        /// <summary>
        ///     Remplace dans la chaine d'entrée tous les caractères qui matchent par la nouvelle valeur
        /// </summary>
        /// <param name="input">Chaîne à nettoyer</param>
        /// <param name="regexType">Type de regex à appliquer</param>
        /// <param name="replacement">Valeur de remplacment</param>
        /// <returns>Chaine nettoyée</returns>
        public static string Replace(this string input, RegexType regexType, string replacement)
        {
            return Regex.Replace(input, regexType.ToDescription(), replacement);
        }
    }
}