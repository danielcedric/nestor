using System.Security.Cryptography;
using System.Text;

namespace Nestor.Tools.Helpers
{
    public class SHA1Helper
    {
        public static string Hash(string input)
        {
            var sha = SHA1.Create();
            var bHash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();

            for (var i = 0; i < bHash.Length; i++) sBuilder.Append(bHash[i].ToString("x2"));

            return sBuilder.ToString();
        }
    }
}