using System;
using System.Linq;

namespace Nestor.Tools.Helpers
{
    public static class PrimitiveTypesHelper
    {
        private static readonly Type[] primitiveTypes;

        /// <summary>
        ///     Constructeur statique
        /// </summary>
        static PrimitiveTypesHelper()
        {
            var types = new[]
            {
                typeof(Enum),
                typeof(string),
                typeof(char),
                typeof(Guid),

                typeof(bool),
                typeof(byte),
                typeof(short),
                typeof(int),
                typeof(long),
                typeof(float),
                typeof(double),
                typeof(decimal),

                typeof(sbyte),
                typeof(ushort),
                typeof(uint),
                typeof(ulong),

                typeof(DateTime),
                typeof(DateTimeOffset),
                typeof(TimeSpan)
            };


            var nullTypes = from t in types
                where t.IsValueType
                select typeof(Nullable<>).MakeGenericType(t);

            primitiveTypes = types.Concat(nullTypes).ToArray();
        }

        /// <summary>
        ///     Obtient un booléen qui indique si le type passé en paramètre est un type "valeur" ou "struct"
        /// </summary>
        /// <param name="type">type à tester</param>
        /// <returns></returns>
        public static bool IsByValueType(Type type)
        {
            if (primitiveTypes.Any(x => x.IsAssignableFrom(type)))
                return true;

            var nut = Nullable.GetUnderlyingType(type);
            return nut != null && nut.IsEnum;
        }
    }
}