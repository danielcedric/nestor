using Nestor.Tools.Languages;
using System;

namespace Nestor.Tools.Algorithms
{
    /// <summary>
    /// Type de vérification par l'algorithme de Luhn.
    /// </summary>
    public enum LuhnCheckType
    {
        /// <summary>
        /// Aucun.
        /// </summary>
        None = 0,
        /// <summary>
        /// Carte de crédit.
        /// </summary>
        CreditCard = 1,
        /// <summary>
        /// Siren.
        /// </summary>
        Siren = 2,
        /// <summary>
        /// Siret.
        /// </summary>
        Siret = 3
    }


    /// <summary>
    /// Algorithme de Luhn.
    /// </summary>
    /// <remarks>L'algorithme de Luhn est décrit ici :
    /// http://en.wikipedia.org/wiki/Luhn_algorithm.
    /// </remarks>
    public static class Luhn
    {

        /// <summary>
        /// Utilise l'algorithme de Luhn pour vérifier la validité d'un nombre donné.
        /// </summary>
        /// <param name="number">Nombre à vérifier.</param>
        /// <returns>Vrai si le nombre est valide selon l'algorithme de Luhn, faux sinon.</returns>
        public static bool Check(long number)
        {
            return Luhn.Check(number.ToString(), -1);
        }


        /// <summary>
        /// Utilise l'algorithme de Luhn pour vérifier la validité d'un nombre donné.
        /// </summary>
        /// <param name="number">Nombre à vérifier.</param>
        /// <returns>Vrai si le nombre est valide selon l'algorithme de Luhn, faux sinon.</returns>
        public static bool Check(string number)
        {
            return Luhn.Check(number, -1);
        }


        /// <summary>
        /// Utilise l'algorithme de Luhn pour vérifier la validité d'un nombre donné.
        /// </summary>
        /// <param name="number">Nombre à vérifier.</param>
        /// <param name="length">Longueur de la chaîne.</param>
        /// <returns>Vrai si le nombre est valide selon l'algorithme de Luhn, faux sinon.</returns>
        private static bool Check(string number, int length)
        {
            #region Exceptions

            // La chaîne ne doit pas être vide :
            if (string.IsNullOrEmpty(number))
                throw new ArgumentNullException(ExceptionMessage.StringNullOrEmpty);
            // La chaîne doit être de la longueur donnée :
            if (length > -1 && number.Length != length)
                throw new ArgumentException(string.Format(ExceptionMessage.StringWrongLength, length));
            // La chaîne doit être un nombre :
            long result = 0;
            if (!long.TryParse(number, out result))
                throw new ArgumentException(ExceptionMessage.StringNotANumber);
            // Le nombre doit être supérieur à zéro :
            if (result <= 0)
                throw new ArgumentOutOfRangeException(
                    "number",
                    string.Format(ExceptionMessage.NumberMustBeStrictlyPositive, "number"));

            #endregion

            int numberLength = number.Length;
            int figure = 0;
            int total = 0;
            int position = 1;

            for (int i = numberLength; i > 0; i--)
            {
                // Récupérer le chiffre en position courante :
                figure = int.Parse(number[i - 1].ToString());

                // Multiplier ce chiffre par 2 ou 1 en fonction de sa position :
                figure = figure * ((position % 2 == 0) ? 2 : 1);

                // Retirer 9 si le résultat obtenu est > 10
                // (équivaut à ajouter le chiffre des dizaines et celui des unités) :
                figure = (figure >= 10) ? figure - 9 : figure;

                total += figure;
                position++;
            }

            // Le chiffre valide pour la formule de Luhn, s'il est congru au modulo 10 :
            return (total % 10 == 0);
        }


        /// <summary>
        /// Utilise l'algorithme de Luhn pour vérifier la validité d'un nombre donné.
        /// </summary>
        /// <param name="number">Nombre à vérifier.</param>
        /// <param name="checkType">Type de vérification.</param>
        /// <returns>Vrai si le nombre est valide selon l'algorithme de Luhn, faux sinon.</returns>
        public static bool Check(long number, LuhnCheckType checkType)
        {
            return Luhn.Check(number.ToString(), checkType);
        }


        /// <summary>
        /// Utilise l'algorithme de Luhn pour vérifier la validité d'un nombre donné.
        /// </summary>
        /// <param name="number">Nombre à vérifier.</param>
        /// <param name="checkType">Type de vérification.</param>
        /// <returns>Vrai si le nombre est valide selon l'algorithme de Luhn, faux sinon.</returns>
        public static bool Check(string number, LuhnCheckType checkType)
        {
            switch (checkType)
            {
                case LuhnCheckType.CreditCard:
                    return Luhn.Check(number, 16);
                case LuhnCheckType.Siren:
                    return Luhn.Check(number, 9);
                case LuhnCheckType.Siret:
                    return Luhn.Check(number, 14);
                case LuhnCheckType.None:
                default:
                    return Luhn.Check(number, -1);
            }
        }

    }
}
