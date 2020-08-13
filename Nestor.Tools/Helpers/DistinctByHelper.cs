using System;
using System.Collections.Generic;

namespace Nestor.Tools.Helpers
{
    public static class DistinctByHelper
    {
        /// <summary>
        ///     Effectue un distinct sur la condition passé en paramètre
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            var knownKeys = new HashSet<TKey>();
            foreach (var element in source)
                if (knownKeys.Add(keySelector(element)))
                    yield return element;
        }
    }
}