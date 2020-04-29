using System;

namespace Nestor.Tools.Algorithms
{
    /// <summary>
    /// Fournit des méthodes permettant d'évaluer la similarité entre 2 chaînes de caractères,
    /// selon l'algorithme de la distance de Levenshtein.
    /// </summary>
    /// <remarks>L'algorithme de la distance de Levenshtein est décrit ici :
    /// http://en.wikipedia.org/wiki/Levenshtein_distance.
    /// </remarks>
    public static class Levenshtein
    {

        /// <summary>
        /// Calcule la distance de Levenshtein entre 2 chaînes de caractères.
        /// </summary>
        /// <param name="a">Première chaîne à comparer.</param>
        /// <param name="b">Seconde chaîne à comparer.</param>
        /// <param name="caseSensitive">true pour tenir compte de la casse, false sinon.</param>
        /// <returns>La distance de Levenshtein entre les 2 chaînes.</returns>
        /// <remarks>
        /// <list type="bullet">
        /// <item>La distance de Levenshtein est toujours supérieure ou égale à la différence de longueur entre les 2 chaines</item>
        /// <item>La distance de Levenshtein est toujours inférieure ou égale à la longueur de la plus longue chaine</item>
        /// <item>La distance de Levenshtein entre 2 chaines identiques est 0</item>
        /// </list>
        /// </remarks>
        public static int ComputeDistance(string a, string b, bool caseSensitive)
        {
            if (!caseSensitive)
            {
                a = a.ToLower();
                b = b.ToLower();
            }

            int m = a.Length;
            int n = b.Length;
            int[,] d = new int[m + 1, n + 1];

            for (int i = 0; i <= m; i++)
                d[i, 0] = i;
            for (int i = 0; i <= n; i++)
                d[0, i] = i;

            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    int cost;
                    if (a[i - 1] == b[j - 1])
                        cost = 0;
                    else
                        cost = 1;
                    d[i, j] = Min(d[i - 1, j] + 1,
                                    d[i, j - 1] + 1,
                                    d[i - 1, j - 1] + cost);
                }
            }

            return d[m, n];
        }

        /// <summary>
        /// Calcule le coefficient de corrélation entre 2 chaînes, sur la base de la distance de Levenshtein
        /// </summary>
        /// <param name="a">Première chaîne à comparer</param>
        /// <param name="b">Seconde chaîne à comparer</param>
        /// <param name="caseSensitive">true pour tenir compte de la casse, false sinon</param>
        /// <returns>Le coefficient de corrélation entre les 2 chaînes. Cette valeur est comprise entre 0 (chaînes complètement différentes) et 1 (chaînes identiques)</returns>
        /// <remarks>Ce coefficient est calculé selon la formule suivante : <c>1 - d/L</c>, où <c>d</c> est la distance de Levenshtein entre les 2 chaînes, et <c>L</c> la longueur de la plus longue chaîne.</remarks>
        public static double ComputeCorrelation(string a, string b, bool caseSensitive)
        {
            int distance = ComputeDistance(a, b, caseSensitive);
            int longest = Max(a.Length, b.Length);
            return 1 - (double)distance / longest;
        }


        private static T Min<T>(T arg0, params T[] args) where T : IComparable
        {
            T min = arg0;
            for (int i = 0; i < args.Length; i++)
            {
                T x = args[i];
                if (x.CompareTo(min) < 0)
                    min = x;
            }
            return min;
        }

        private static T Max<T>(T arg0, params T[] args) where T : IComparable
        {
            T max = arg0;
            for (int i = 0; i < args.Length; i++)
            {
                T x = args[i];
                if (x.CompareTo(max) > 0)
                    max = x;
            }
            return max;
        }
    }
}
