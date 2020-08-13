using System.Text;
using System.Text.RegularExpressions;

namespace Nestor.Tools.Helpers
{
    public class SoundexHelper
    {
        private static readonly string _notAcceptedLetters = "A|À|Â|Ä|E|É|È|Ê|Ë|I|Ï|Î|O|Ô|Ö|U|Ù|Û|Ü|Y|H|W";

        /// <summary>
        ///     Remove the accent.
        /// </summary>
        /// <param name="c"> The char to check. </param>
        /// <returns> The char without accent. </returns>
        private static char GetNotAccent(char c)
        {
            switch (c)
            {
                case 'À':
                case 'Â':
                case 'Ä': return 'A';
                case 'Ç': return 'S';
                case 'É':
                case 'È':
                case 'Ê':
                case 'Ë': return 'E';
                case 'Ï':
                case 'Î': return 'I';
                case 'Ô':
                case 'Ö': return 'O';
                case 'Ù':
                case 'Û':
                case 'Ü': return 'U';
            }

            return c;
        }

        /// <summary>
        ///     Transforme le mot en entrée en code Soundex
        /// </summary>
        /// <param name="wordToSoundex">mot à transformer</param>
        /// <returns>Code Soundex sur 4 caractères</returns>
        public static string GetSoundExCode(string wordToSoundex)
        {
            var originalWord = Regex.Replace(wordToSoundex.Trim().ToUpper().RemoveSpecialCharacters(), @"(\s*)(-*)",
                string.Empty);

            if (string.IsNullOrEmpty(originalWord))
                return string.Empty;

            var sb = new StringBuilder();

            // Remove the accents
            foreach (var c in originalWord)
                sb.Append(GetNotAccent(c));

            // Replace...
            sb.Replace("GUI", "KI").Replace("GUE", "KE").Replace("GA", "KA").Replace("GO", "KO");
            sb.Replace("GU", "K").Replace("CA", "KA").Replace("CO", "KO").Replace("CU", "KU");
            sb.Replace("Q", "K").Replace("CC", "K").Replace("CK", "K");

            // Remove not accepted letters
            var word = sb.ToString();
            word = word[0] + Regex.Replace(word.Length > 1 ? word.Substring(1) : string.Empty, _notAcceptedLetters,
                "A");
            sb = new StringBuilder(word);

            // Replace...
            sb.Replace("MAC", "MCC").Replace("ASA", "AZA").Replace("KN", "NN").Replace("PF", "FF");
            sb.Replace("SCH", "SSS").Replace("PH", "FF");

            // Replace...
            word = Regex.Replace(sb.ToString(), "([^C|^S])H", "$1");
            word = Regex.Replace(word, "([^A])Y", "$1");
            word = Regex.Replace(word, "(.*)[A|D|T|S]$", "$1");

            var replace = Regex.Replace(word.Length > 1 ? word.Substring(1) : string.Empty, "A", string.Empty);
            if (!string.IsNullOrEmpty(word))
                word = word[0] + replace;
            word = Regex.Replace(word, @"(\D)\1+", "$1");

            // Pad Left with empty char if needed
            return word.Length > 4 ? word.Substring(0, 4) : word.PadRight(4, ' ');
        }
    }
}