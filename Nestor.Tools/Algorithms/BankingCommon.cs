using System.Text;

namespace Nestor.Tools.Algorithms
{
    internal static class BankingCommon
    {
        public static string CleanInput(string input)
        {
            var sb = new StringBuilder();
            foreach (var c in input)
                if (char.IsDigit(c) ||
                    c >= 'a' && c <= 'z' ||
                    c >= 'A' && c <= 'Z')
                    sb.Append(char.ToUpper(c));
            return sb.ToString();
        }
    }
}