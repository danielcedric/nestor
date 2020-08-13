using System.Collections;
using System.Text;

namespace Nestor.Tools.Helpers
{
    public class ReflectionHelper
    {
        /// <summary>
        ///     Obtient dans un dictionnaire clé / valeur l'ensemble des propriétés et des valeurs de l'objet passé en paramètre
        /// </summary>
        /// <param name="obj">Objet à inspecter</param>
        /// <returns></returns>
        public static string PrintProperties(object obj, int indent)
        {
            if (obj == null) return null;

            var ret = new StringBuilder();

            var indentString = new string(' ', indent);
            var objType = obj.GetType();
            var properties = objType.GetProperties();
            foreach (var property in properties)
            {
                var propValue = property.GetValue(obj, null);
                var elems = propValue as IList;
                if (elems != null)
                {
                    foreach (var item in elems)
                    {
                        ret.AppendLine(string.Format("{0}{1}[{2}]:", indentString, property.Name, elems.IndexOf(item)));
                        ret.AppendLine(PrintProperties(item, indent + 3));
                    }
                }
                else
                {
                    // This will not cut-off System.Collections because of the first check
                    if (property.PropertyType.Assembly == objType.Assembly)
                    {
                        ret.AppendLine(string.Format("{0}{1}: {2}", indentString, property.Name,
                            propValue != null ? string.Empty : "<null>"));
                        if (propValue != null)
                            ret.AppendLine(PrintProperties(propValue, indent + 2));
                    }
                    else
                    {
                        ret.AppendLine(string.Format("{0}{1}: {2}", indentString, property.Name,
                            propValue != null ? propValue : "<null>"));
                    }
                }
            }

            return ret.ToString();
        }
    }
}