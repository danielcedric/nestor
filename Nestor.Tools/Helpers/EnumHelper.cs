using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Nestor.Tools.Helpers
{
    public static class EnumHelper
    {
        /// <summary>
        /// Obtient le contenu de l'attribut [Description] de l'énuméaration passée en paramètre
        /// </summary>
        /// <param name="value">Valeur de l'énumération</param>
        /// <returns>contenu de l'attribut description</returns>
        public static string ToDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        /// <summary>
        /// Obtient le contenu de l'énumération dont le type est passé en paramètre
        /// </summary>
        /// <typeparam name="T">Typde de l'énumération</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> ToList<T>() where T : struct, IConvertible
        {
            Type enumType = typeof(T);

            // Can't use generic type constraints on value types,
            // so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = Enum.GetValues(enumType);
            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));

            return enumValList;
        }

        /// <summary>
        /// Obtient le contenu de l'énumération dont le type est passé en paramètre.
        /// </summary>
        /// <typeparam name="T">Typde de l'énumération</typeparam>
        /// <returns>Liste des descriptions de l'énumération</returns>
        public static IEnumerable<string> ToDescriptionList<T>() where T : struct, IConvertible
        {
            List<string> enumValDescriptionList = new List<string>();
            foreach (T val in ToList<T>())
                enumValDescriptionList.Add(ToDescription(val as Enum));

            return enumValDescriptionList;
        }

        /// <summary>
        /// Obtient le contenu de l'énumation sous forme d'un dictionnaire entier / valeur string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<int, string>> ToDictionary<T>() where T : struct, IConvertible
        {
            List<KeyValuePair<int, string>> enumValDescriptionList = new List<KeyValuePair<int, string>>();
            foreach (T val in ToList<T>())
                enumValDescriptionList.Add(new KeyValuePair<int, string>(Convert.ToInt32(val as Enum), ToDescription(val as Enum)));

            return enumValDescriptionList;
        }
    }

    public static class EnumHelper<T>
    {
        public static IList<T> GetValues(Enum value)
        {
            var enumValues = new List<T>();

            foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
            }
            return enumValues;
        }

        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static IList<string> GetNames(Enum value)
        {
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        public static IList<string> GetDisplayValues(Enum value)
        {
            return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        }

        public static string GetDisplayValue(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var displayAttribute = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (displayAttribute == null) return string.Empty;
            return (displayAttribute.Length > 0) ? displayAttribute[0].Name : value.ToString();
        }

        public static IList<string> GetDisplayShortNames(Enum value)
        {
            return GetNames(value).Select(obj => GetDisplayShortName(Parse(obj))).ToList();
        }

        public static string GetDisplayShortName(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var displayAttribute = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (displayAttribute == null) return string.Empty;
            return (displayAttribute.Length > 0) ? displayAttribute[0].ShortName : value.ToString();
        }
    }
}
