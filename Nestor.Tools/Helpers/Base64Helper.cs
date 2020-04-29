using System;

namespace Nestor.Tools.Helpers
{
    public class Base64Helper
    {
        /// <summary>
        /// Encode une chaine de caractère en Base64
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Décode une chaine de caractère au format Base64
        /// </summary>
        /// <param name="base64EncodedData"></param>
        /// <returns></returns>
        public static string Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Retourne vrai si la chaine de caractère passée en paramètre est une chaine B64 valide
        /// </summary>
        /// <param name="base64EncodedData"></param>
        /// <returns></returns>
        public static bool IsValidBase64String(string base64EncodedData)
        {
            try
            {
                System.Convert.FromBase64String(base64EncodedData);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
