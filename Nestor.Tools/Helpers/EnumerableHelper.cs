using System;
using System.Collections.Generic;

namespace Nestor.Tools.Helpers
{
    public static class EnumerableHelper
    {
        #region Scan

        /// <summary>
        ///     Applique une fonction d'accumulation sur une séquence, en renvoyant la valeur
        ///     de l'accumulateur à chaque étape.
        /// </summary>
        /// <typeparam name="TSource">Type des éléments de <c>source</c></typeparam>
        /// <typeparam name="TAccumulate">Type de l'accumulateur</typeparam>
        /// <param name="source">Séquence sur laquelle appliquer l'accumulation</param>
        /// <param name="seed">Valeur initiale de l'accumulateur</param>
        /// <param name="func">Fonction d'accumulation à appeler sur chaque élément</param>
        /// <returns>Séquence des valeurs de l'accumulateur à chaque étape.</returns>
        /// <remarks>
        ///     Cette méthode est similaire à Enumerable.Aggregate, mais cette dernière ne renvoie que le résultat
        ///     final, alors que Scan renvoie le résultat de chaque étape.
        /// </remarks>
        public static IEnumerable<TAccumulate> Scan<TSource, TAccumulate>(
            this IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> func)
        {
            return source.ScanIterator(seed, func);
        }

        /// <summary>
        ///     Applique une fonction d'accumulation sur une séquence, en renvoyant
        ///     la valeur de l'accumulateur à chaque étape.
        /// </summary>
        /// <typeparam name="TSource">Type des éléments de <c>source</c></typeparam>
        /// <typeparam name="TAccumulate">Type de l'accumulateur</typeparam>
        /// <param name="source">Séquence sur laquelle appliquer l'accumulation</param>
        /// <param name="func">Fonction d'accumulation à appeler sur chaque élément</param>
        /// <returns>Séquence des valeurs de l'accumulateur à chaque étape.</returns>
        /// <remarks>
        ///     Cette méthode est similaire à Enumerable.Aggregate, mais cette dernière ne renvoie que le résultat
        ///     final, alors que Scan renvoie le résultat de chaque étape.
        /// </remarks>
        public static IEnumerable<TAccumulate> Scan<TSource, TAccumulate>(
            this IEnumerable<TSource> source,
            Func<TAccumulate, TSource, TAccumulate> func)
        {
            return source.ScanIterator(default, func);
        }

        private static IEnumerable<TAccumulate> ScanIterator<TSource, TAccumulate>(
            this IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> func)
        {
            var previous = seed;
            foreach (var item in source)
            {
                var result = func(previous, item);
                previous = result;
                yield return result;
            }
        }

        /// <summary>
        ///     Applique une fonction d'accumulation sur une séquence, en renvoyant
        ///     la valeur de l'accumulateur à chaque étape. La fonction d'accumulation
        ///     prend en paramètre les 2 valeurs précédentes de l'accumulateur.
        /// </summary>
        /// <typeparam name="TSource">Type des éléments de <c>source</c></typeparam>
        /// <typeparam name="TAccumulate">Type de l'accumulateur</typeparam>
        /// <param name="source">Séquence sur laquelle appliquer l'accumulation</param>
        /// <param name="seed1">Valeur initiale de l'accumulateur (itération n-1)</param>
        /// <param name="seed2">Valeur initiale de l'accumulateur (itération n-2)</param>
        /// <param name="func">Fonction d'accumulation à appeler sur chaque élément</param>
        /// <returns>Séquence des valeurs de l'accumulateur à chaque étape.</returns>
        public static IEnumerable<TAccumulate> Scan<TSource, TAccumulate>(
            this IEnumerable<TSource> source,
            TAccumulate seed1,
            TAccumulate seed2,
            Func<TAccumulate, TAccumulate, TSource, TAccumulate> func)
        {
            return source.ScanIterator(seed1, seed2, func);
        }

        /// <summary>
        ///     Applique une fonction d'accumulation sur une séquence, en renvoyant
        ///     la valeur de l'accumulateur à chaque étape. La fonction d'accumulation
        ///     prend en paramètre les 2 valeurs précédentes de l'accumulateur.
        /// </summary>
        /// <typeparam name="TSource">Type des éléments de <c>source</c></typeparam>
        /// <typeparam name="TAccumulate">Type de l'accumulateur</typeparam>
        /// <param name="source">Séquence sur laquelle appliquer l'accumulation</param>
        /// <param name="func">Fonction d'accumulation à appeler sur chaque élément</param>
        /// <returns>Séquence des valeurs de l'accumulateur à chaque étape.</returns>
        public static IEnumerable<TAccumulate> Scan<TSource, TAccumulate>(
            this IEnumerable<TSource> source,
            Func<TAccumulate, TAccumulate, TSource, TAccumulate> func)
        {
            return source.ScanIterator(default, default, func);
        }

        private static IEnumerable<TAccumulate> ScanIterator<TSource, TAccumulate>(
            this IEnumerable<TSource> source,
            TAccumulate seed1,
            TAccumulate seed2,
            Func<TAccumulate, TAccumulate, TSource, TAccumulate> func)
        {
            var previous1 = seed1;
            var previous2 = seed2;
            foreach (var item in source)
            {
                var result = func(previous1, previous2, item);
                previous2 = previous1;
                previous1 = result;
                yield return result;
            }
        }

        #endregion
    }
}