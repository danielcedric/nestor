using System.Security.Cryptography;
using System.Text;

namespace Nestor.Tools.Helpers
{
    public class SHA1Helper
    {
        public static string Hash(string input)
        {
            SHA1 sha = SHA1.Create();
            byte[] bHash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < bHash.Length; i++)
            {
                sBuilder.Append(bHash[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
