using System.Text.RegularExpressions;

namespace Nestor.Tools.Domain.Helpers
{
    public static class NamespaceHelper
    {
        public const string DOMAIN_NAMESPACE_PATTERN = "(?<=DataCrafter.).*?(?=.Domain)";// "(?<=Business.).*?(?=.Domain)";

        /// <summary>
        ///     Extrait le nom du domaine depuis l'espace de nom
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        public static string ExtractSchemaFromDomain(this string ns, string domainNamespacePattern = "(?<=DataCrafter.).*?(?=.Domain)")
        {
            if (Regex.IsMatch(ns, domainNamespacePattern??DOMAIN_NAMESPACE_PATTERN))
                return Regex.Match(ns, domainNamespacePattern ?? DOMAIN_NAMESPACE_PATTERN).Value.ToLowerInvariant();

            return "dbo";
        }
    }
}