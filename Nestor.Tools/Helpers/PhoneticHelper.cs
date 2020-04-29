using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Nestor.Tools.Helpers
{
    public class PhoneticHelper
    {

        public static double ToPhonex(string input)
        {
            if (string.IsNullOrEmpty(input))
                return 0;

            // 1. Suppression des caractères spéciaux et passage du résultat en majuscule
            string text = StringHelper.RemoveDiacritics(input).ToUpper();

            // 2. Remplacement des Y par de I
            text = text.Replace('Y', 'I');

            // 3. Supprimer les H qui ne sont pas précédés de C, S, ou P
            text = Regex.Replace(text, "(?-i:(?<=[^PCS])H)", string.Empty);

            // 4. Remplacement du son PH par F
            text = text.Replace("PH", "F");

            // 5. Remplacement des occurences suivantes si elles sont suivies par une lettre A,E,I,O,U,N
            text = Regex.Replace(text, "[A|E]I[N|M](?=[A|E|I|O|U])", "YN");

            // 6. Remplacement des sons ('o','eau','oua','ein')
            text = text.Replace("EAU", "O")
                .Replace("OUA", "2")
                .Replace("EIN", "4")
                .Replace("AIN", "4")
                .Replace("EIM", "4")
                .Replace("AIM", "4");

            // 7. Remplacement du son 'é'
            text = text.Replace("AI", "Y")
                .Replace("EI", "Y")
                .Replace("ER", "YR")
                .Replace("ESS", "YS")
                .Replace("ET", "YT")
                .Replace("EZ", "YZ");

            // 8. Remplacement les groupes de 2 lettres suivantes (son ‘an' et ‘in'), sauf s'il sont suivi par une lettre a, e, i o, u ou un son 1 à 4 :
            text = Regex.Replace(text, "AN(?=[^A|E|I|O|U|1|2|3|4])", "1");
            text = Regex.Replace(text, "ON(?=[^A|E|I|O|U|1|2|3|4])", "1");
            text = Regex.Replace(text, "AM(?=[^A|E|I|O|U|1|2|3|4])", "1");
            text = Regex.Replace(text, "EN(?=[^A|E|I|O|U|1|2|3|4])", "1");
            text = Regex.Replace(text, "EM(?=[^A|E|I|O|U|1|2|3|4])", "1");
            text = Regex.Replace(text, "IN(?=[^A|E|I|O|U|1|2|3|4])", "4");

            // 9. Remplacement du S par Z s'ils sont suivis et précédés des lettres a,e,i,o,u ou d'un son 1 à 4.
            text = Regex.Replace(text, "(?<=[A|E|I|O|U|Y|1|2|3|4])S(?=[A|E|I|O|U|Y|1|2|3|4])", "Z");

            // 10. Remplacement des groupes de lettres suivants: 
            text = text.Replace("OE", "E")
                .Replace("EU", "E")
                .Replace("AU", "O")
                .Replace("OI", "2")
                .Replace("OY", "2")
                .Replace("OU", "3");

            // 11. Remplacement du C par un S s'il est suivi d'un e ou d'un i
            text = Regex.Replace(text, "C(?=[E|I])", "S");

            // 12 . Remplacement des groupes de lettres suivant
            text = text.Replace("CH", "5")
                .Replace("SCH", "5")
                .Replace("SH", "5")
                .Replace("SS", "S");

            // 13. Remplacement des groupes de lettres suivantes 
            text = text.Replace("C", "K")
                .Replace("Q", "K")
                .Replace("QU", "K")
                .Replace("GU", "K")
                .Replace("GA", "KA")
                .Replace("GO", "KO")
                .Replace("GY", "KY")
                .Replace("A", "O")
                .Replace("D", "T")
                .Replace("P", "T")
                .Replace("J", "G")
                .Replace("B", "F")
                .Replace("V", "F")
                .Replace("M", "N");

            // 14. Remplacement des lettres dupliquées
            text = Regex.Replace(text, @"(\D)\1+", "$1");

            // 15. Suppression des terminaisons t,x
            text = Regex.Replace(text, "[T|X]$", string.Empty);

            // 16. Affectation à chaque lettre du code numérique correspondant en partant de la dernière lettre
            List<char> num = new List<char>() { '1', '2', '3', '4', '5', 'E', 'F', 'G', 'H', 'I', 'K', 'L', 'N', 'O', 'R', 'S', 'T', 'U', 'W', 'X', 'Y', 'Z' };
            List<int> r = new List<int>(text.Length);
            for (int i = 0; i < text.Length; i++)
                r.Add(num.IndexOf(text[i]));

            // 17. Conversion des codes numériques ainsi obtenu en un nombre de base 22 exprimé en virgule flottante.
            double result = 0.0;
            int j = 1;
            foreach (var code in r)
            {
                result += Math.Pow((double)(code * 22), (double)(-j));
                j += 1;
            }

            return result;
        }
    }
}
