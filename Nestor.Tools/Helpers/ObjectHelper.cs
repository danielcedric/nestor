using System.Collections.Generic;
using System.Dynamic;

namespace Nestor.Tools.Helpers
{
    public static class ObjectHelper
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items) collection.Add(item);
        }

        /// <summary>
        ///     Convertit un dictionnaire de clé valeur en objet dynamique
        /// </summary>
        /// <param name="source">Object source à convertir</param>
        /// <returns>Object dynamique</returns>
        public static dynamic ToDynamicObject(this IDictionary<string, object> source)
        {
            ICollection<KeyValuePair<string, object>> someObject = new ExpandoObject();
            someObject.AddRange(source);
            return someObject;
        }
    }
}