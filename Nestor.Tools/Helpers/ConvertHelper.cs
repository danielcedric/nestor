using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Nestor.Tools.Helpers
{
    public static class ConvertHelper
    {
        public static byte[] ToByteArray(this string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string ToString(this byte[] bytes)
        {
            var chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        ///     Convertit une instance d'un type en un objet dynamique
        /// </summary>
        /// <typeparam name="T">Type de l'objet à convertir</typeparam>
        /// <param name="obj">Objet à convertir</param>
        /// <returns>Objet dynamique convertit</returns>
        public static dynamic ToDynamic<T>(this T obj)
        {
            //Création d'un dictionnaire de clé valeur
            IDictionary<string, object> expando = new ExpandoObject();

            //Récupération de toutes les propriétés du type
            foreach (var propertyInfo in typeof(T).GetProperties())
                //Ajout d'une clé valeur
                expando.Add(propertyInfo.Name, propertyInfo.GetValue(obj));
            return expando as ExpandoObject;
        }
    }
}