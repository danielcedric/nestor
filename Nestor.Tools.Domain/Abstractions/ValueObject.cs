using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nestor.Tools.Attributes;
using Nestor.Tools.Domain.Helpers;

namespace Nestor.Tools.Domain.Abstractions
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        private List<FieldInfo> fields;
        private List<PropertyInfo> properties;

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(ValueObject obj)
        {
            return Equals(obj as object);
        }

        /// <summary>
        ///     Surcharge de l'opérateur d'égalité
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool operator ==(ValueObject obj1, ValueObject obj2)
        {
            if (Equals(obj1, null))
            {
                if (Equals(obj2, null))
                    return true;
                return false;
            }

            return obj1.Equals(obj2);
        }

        public static bool operator !=(ValueObject obj1, ValueObject obj2)
        {
            return !(obj1 == obj2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return GetProperties().All(p => PropertiesAreEqual(obj, p))
                   && GetFields().All(f => FieldsAreEqual(obj, f));
        }

        private bool PropertiesAreEqual(object obj, PropertyInfo p)
        {
            return Equals(p.GetValue(this, null), p.GetValue(obj, null));
        }

        private bool FieldsAreEqual(object obj, FieldInfo f)
        {
            return Equals(f.GetValue(this), f.GetValue(obj));
        }

        private IEnumerable<PropertyInfo> GetProperties()
        {
            if (properties == null)
                properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => !Attribute.IsDefined(p, typeof(IgnoreMemberAttribute))).ToList();

            return properties;
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            if (fields == null)
                fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)
                    .Where(f => !Attribute.IsDefined(f, typeof(IgnoreMemberAttribute))).ToList();

            return fields;
        }

        public override int GetHashCode()
        {
            var values = new List<object>();

            values.AddRange(GetProperties().Select(prop => prop.GetValue(this, null)));
            values.AddRange(GetFields().Select(field => field.GetValue(this)));

            return HashCodeHelper.GetHashCode(values);
        }
    }
}