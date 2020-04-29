using System;
using System.Linq;

namespace Nestor.Tools.Helpers
{
    /// <summary>
    /// Cette classe fournit des outils pour effectuer des calculs sur des grands nombres,
    /// au delà de la limite des 64 bits d'un Int64
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Calcule le modulo (reste de la division entière) d'un grand nombre par
        /// un nombre
        /// </summary>
        /// <param name="number">Le grand nombre à diviser</param>
        /// <param name="k">Le diviseur</param>
        /// <returns>le modulo de la division</returns>
        public static ulong Modulo(string number, ulong k)
        {
            // Algorithme de calcul du modulo d'un grand nombre
            // Ai = (10^i) mod k, soit :
            //   A0 = 1; A(i+1) = {Ai * 10} mod k
            // D = Sum(Di * (10^i), i = 0 to n)
            // D mod k = Sum(Di*Ai, i = 0 to n) mod k

            int n = number.Length;

            // Séquence Ai
            var A = Enumerable.Range(0, n)
                    .Select(i => (ulong)i)
                    .Scan(
                        (ulong acc, ulong i) =>
                            i < 1 ? 1 : (acc * 10) % k
                    );

            // Séquence Di
            var D = number.ToCharArray()
                    .Reverse()
                    .Select(ch => ulong.Parse(ch.ToString()));

            var sum = Enumerable.Zip(D, A, (d, a) => d * a)
                      .Aggregate((ulong)0, (s, p) => s + p);

            return sum % k;
        }

        /// <summary>
        /// Obtient l'ecart exprimé en pourcentage entre la valeur a et b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double GetPercentageDiff(double a, double b, int decimals = 2)
        {
            var result = 100 - Math.Round((double)((double)a / (double)b) * 100, decimals);
            return result;
        }
    }
}
