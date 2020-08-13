using System;
using System.Text;
using System.Text.RegularExpressions;
using Nestor.Tools.Languages;

namespace Nestor.Tools.Algorithms
{
    /// <summary>
    ///     Vérification d'un numéro RIB
    /// </summary>
    public static class RIB
    {
        private static readonly Regex ribRegex = new Regex(@"^(?<B>\d{5})(?<G>\d{5})(?<C>[A-Za-z0-9]{11})(?<K>\d{2}$)",
            RegexOptions.Compiled);

        /// <summary>
        ///     Vérifie la validité d'un RIB
        /// </summary>
        /// <param name="number">Le RIB à vérifier</param>
        /// <returns>true si le RIB est valide, false sinon</returns>
        public static bool Check(string number)
        {
            // Suppression des espaces et tirets
            var tmp = BankingCommon.CleanInput(number);

            // Vérification du format BBBBBGGGGGCCCCCCCCCCCKK
            // B : banque
            // G : guichet
            // C : numéro de compte
            // K : clé RIB

            var m = ribRegex.Match(tmp);
            if (!m.Success)
                return false;

            // Extraction des composants
            var b_s = m.Groups["B"].Value;
            var g_s = m.Groups["G"].Value;
            var c_s = m.Groups["C"].Value;
            var k_s = m.Groups["K"].Value;

            // Remplacement des lettres par des chiffres dans le numéro de compte
            var sb = new StringBuilder();
            foreach (var ch in c_s)
                if (char.IsDigit(ch))
                    sb.Append(ch);
                else
                    sb.Append(RibLetterToDigit(ch));
            c_s = sb.ToString();

            // Séparation du numéro de compte pour tenir sur 32bits
            var d_s = c_s.Substring(0, 6);
            c_s = c_s.Substring(6, 5);

            // Calcul de la clé RIB
            // Algo ici : http://fr.wikipedia.org/wiki/Clé_RIB#Algorithme_de_calcul_qui_fonctionne_avec_des_entiers_32_bits

            var b = int.Parse(b_s);
            var g = int.Parse(g_s);
            var d = int.Parse(d_s);
            var c = int.Parse(c_s);
            var k = int.Parse(k_s);

            var calculatedKey = 97 - (89 * b + 15 * g + 76 * d + 3 * c) % 97;

            return k == calculatedKey;
        }

        /// <summary>
        ///     Renvoie un numéro RIB à partir d'un numéro IBAN
        /// </summary>
        /// <param name="iban">Numéro IBAN à convertir en RIB</param>
        /// <returns>Le numéro RIB correspondant au IBAN spécifié</returns>
        public static string FromIBAN(string iban)
        {
            return BankingCommon.CleanInput(iban).Substring(4);
        }

        /// <summary>
        ///     Convertit une lettre d'un RIB en un chiffre selon la table suivante :
        ///     1 2 3 4 5 6 7 8 9
        ///     A B C D E F G H I
        ///     J K L M N O P Q R
        ///     _ S T U V W X Y Z
        /// </summary>
        /// <param name="letter">La lettre à convertir</param>
        /// <returns>Le chiffre de remplacement</returns>
        internal static char RibLetterToDigit(char letter)
        {
            letter = char.ToUpper(letter);
            if (letter >= 'A' && letter <= 'I') return (char) (letter - 'A' + '1');
            if (letter >= 'J' && letter <= 'R') return (char) (letter - 'J' + '1');
            if (letter >= 'S' && letter <= 'Z') return (char) (letter - 'S' + '2');
            throw new ArgumentOutOfRangeException("letter", ExceptionMessage.InvalidRIBCharacter);
        }
    }
}