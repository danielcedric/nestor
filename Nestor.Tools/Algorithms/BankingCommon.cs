using System;
using System.Text;

namespace Nestor.Tools.Algorithms
{
    internal static class BankingCommon
    {
        public static string CleanInput(string input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in input)
            {
                if (Char.IsDigit(c) ||
                    c >= 'a' && c <= 'z' ||
                    c >= 'A' && c <= 'Z')
                {
                    sb.Append(Char.ToUpper(c));
                }
            }
            return sb.ToString();
        }
    }
}
