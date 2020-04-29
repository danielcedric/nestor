using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Nestor.Tools.Helpers
{
    public static class SHA256Helper
    {
        public static string HashHMAC(byte[] key, string message)
        {
            var hash = new HMACSHA256(key);
            return HashEncode(hash.ComputeHash(StringEncode(message)));
        }

        //#region Hash Hex Functions
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="keyHex"></param>
        ///// <param name="message"></param>
        ///// <returns></returns>
        //public static string HashHMACHex(string keyHex, string message)
        //{
        //    byte[] hash = HashHMAC(HexDecode(keyHex), StringEncode(message));
        //    return HashEncode(hash);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="keyHex"></param>
        ///// <param name="message"></param>
        ///// <returns></returns>
        //public static string HashHMACHex(byte[] keyHex, string message)
        //{
        //    byte[] hash = HashHMAC(keyHex, StringEncode(message));
        //    return HashEncode(hash);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="innerKeyHex"></param>
        ///// <param name="outerKeyHex"></param>
        ///// <param name="message"></param>
        ///// <returns></returns>
        //public static string HashSHAHex(string innerKeyHex, string outerKeyHex, string message)
        //{
        //    byte[] hash = HashSHA(HexDecode(innerKeyHex), HexDecode(outerKeyHex), StringEncode(message));
        //    return HashEncode(hash);
        //}
        //#endregion

        //#region Hash Functions
        //private static byte[] HashHMAC(byte[] key, byte[] message)
        //{
        //    var hash = new HMACSHA256(key);
        //    return hash.ComputeHash(message);
        //}

        //private static byte[] HashSHA(byte[] innerKey, byte[] outerKey, byte[] message)
        //{
        //    var hash = new SHA256Managed();

        //    // Compute the hash for the inner data first
        //    byte[] innerData = new byte[innerKey.Length + message.Length];
        //    Buffer.BlockCopy(innerKey, 0, innerData, 0, innerKey.Length);
        //    Buffer.BlockCopy(message, 0, innerData, innerKey.Length, message.Length);
        //    byte[] innerHash = hash.ComputeHash(innerData);

        //    // Compute the entire hash
        //    byte[] data = new byte[outerKey.Length + innerHash.Length];
        //    Buffer.BlockCopy(outerKey, 0, data, 0, outerKey.Length);
        //    Buffer.BlockCopy(innerHash, 0, data, outerKey.Length, innerHash.Length);
        //    byte[] result = hash.ComputeHash(data);

        //    return result;
        //}
        //#endregion

        #region Encoding Helpers
        private static byte[] StringEncode(string text)
        {
            var encoding = new ASCIIEncoding();
            return encoding.GetBytes(text);
        }

        private static string HashEncode(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        private static byte[] HexDecode(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber);
            }
            return bytes;
        }
        #endregion
    }
}
