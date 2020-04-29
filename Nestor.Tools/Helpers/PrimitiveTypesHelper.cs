using System;
using System.Linq;

namespace Nestor.Tools.Helpers
{
    public static class PrimitiveTypesHelper
    {
        private static readonly Type[] primitiveTypes = null;

        /// <summary>
        /// Constructeur statique
        /// </summary>
        static PrimitiveTypesHelper()
        {
            var types = new[]
                          {
                              typeof (Enum),
                              typeof (String),
                              typeof (Char),
                              typeof (Guid),

                              typeof (Boolean),
                              typeof (Byte),
                              typeof (Int16),
                              typeof (Int32),
                              typeof (Int64),
                              typeof (Single),
                              typeof (Double),
                              typeof (Decimal),

                              typeof (SByte),
                              typeof (UInt16),
                              typeof (UInt32),
                              typeof (UInt64),

                              typeof (DateTime),
                              typeof (DateTimeOffset),
                              typeof (TimeSpan),
                          };


            var nullTypes = from t in types
                            where t.IsValueType
                            select typeof(Nullable<>).MakeGenericType(t);

            primitiveTypes = types.Concat(nullTypes).ToArray();
        }

        /// <summary>
        /// Obtient un booléen qui indique si le type passé en paramètre est un type "valeur" ou "struct"
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
