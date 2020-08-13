using System;
using System.Collections.Generic;
using System.IO;

namespace Nestor.Tools.Helpers
{
    public class CsvHelper
    {
        private static string separator = ";"; //séparateur par défaut

        /// <summary>
        ///     Lit les lignes d'un fichier au format .csv (plain/text)
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="customseparator"></param>
        /// <param name="takes"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        public static IDictionary<int, List<string>> ReadAllRows(string filePath, string customseparator = "",
            int? takes = 0, int? skip = 0)
        {
            if (!string.IsNullOrEmpty(customseparator))
                separator = customseparator;
            else
                separator = ";";

            var entries = new Dictionary<int, List<string>>();

            using (var file = new StreamReader(filePath, FileHelper.DetectEncoding(filePath)))
            {
                string line = null;
                var rowIndex = 0;
                while ((line = file.ReadLine()) != null)
                {
                    var hasNoLimit = !takes.HasValue && !skip.HasValue;

                    if (hasNoLimit || skip.HasValue && rowIndex >= skip.Value)
                        entries.Add(rowIndex, new List<string>(line.Split(new[] {separator}, StringSplitOptions.None)));

                    rowIndex++;

                    if (takes.HasValue && rowIndex >= takes.Value)
                        break;
                }
            }

            return entries;
        }

        /// <summary>
        ///     Compte le nombre de lignes du fichier
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static int CountRows(string filePath)
        {
            var counter = 0;
            using (var file = new StreamReader(filePath, FileHelper.DetectEncoding(filePath)))
            {
                string line = null;
                while ((line = file.ReadLine()) != null) counter++;
            }

            return counter;
        }
    }
}