using System;
using System.Collections.Generic;
using System.Linq;

namespace Nestor.Tools.Helpers
{
    /// <summary>
    /// Fournit des méthodes d'extension pour le type Random
    /// </summary>
    public static class RandomHelper
    {
        /// <summary>
        /// Renvoie un élément choisi aléatoirement dans la collection spécifiée
        /// </summary>
        /// <typeparam name="T">Type des éléments de la collection</typeparam>
        /// <param name="random">Object Random à utiliser</param>
        /// <param name="items">Collection dans laquelle choisir un élément</param>
        /// <returns>Un élément choisi aléatoirement dans la collection</returns>
        public static T Pick<T>(this Random random, ICollection<T> items)
        {
            int index = random.Next(items.Count);
            return items.ElementAt(index);
        }

        /// <summary>
        /// Renvoie un objet choisi aléatoirement parmi les paramètres spécifiés
        /// </summary>
        /// <typeparam name="T">Type d'objet à choisir</typeparam>
        /// <param name="random">Objet Random à utiliser</param>
        /// <param name="first">Premier paramètre</param>
        /// <param name="second">Second paramètre</param>
        /// <param name="others">Autres paramètres</param>
        /// <returns>Un objet choisi aléatoirement parmi les paramètres spécifiés</returns>
        public static T Pick<T>(this Random random, T first, T second, params T[] others)
        {
            var items = new[] { first, second }.Concat(others).ToArray();
            return random.Pick(items);
        }
    }
}
