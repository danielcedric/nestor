using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Nestor.Tools.Attributes;
using Nestor.Tools.Domain.Helpers;

namespace Nestor.Tools.Domain.Abstractions
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        private List<PropertyInfo> properties;
        private List<FieldInfo> fields;

        /// <summary>
        /// Surcharge de l'opérateur d'égalité
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool operator ==(ValueObject obj1, ValueObject obj2)
        {
            if (object.Equals(obj1, null))
            {
                if (object.Equals(obj2, null))
                    return true;
                else
                    return false;
            }

            return obj1.Equals(obj2);
        }

        public static bool operator !=(ValueObject obj1, ValueObject obj2)
        {
            return !(obj1 == obj2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(ValueObject obj)
        {
            return Equals(obj as object);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return GetProperties().All(p => PropertiesAreEqual(obj, p))
                && GetFields().All(f => FieldsAreEqual(obj, f));
        }

        private bool PropertiesAreEqual(object obj, PropertyInfo p)
        {
            return object.Equals(p.GetValue(this, null), p.GetValue(obj, null));
        }

        private bool FieldsAreEqual(object obj, FieldInfo f)
        {
            return object.Equals(f.GetValue(this), f.GetValue(obj));
        }

        private IEnumerable<PropertyInfo> GetProperties()
        {
            if (this.properties == null)
            {
                this.properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => !Attribute.IsDefined(p, typeof(IgnoreMemberAttribute))).ToList();
            }

            return this.properties;
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            if (this.fields == null)
            {
                this.fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)
                    .Where(f => !Attribute.IsDefined(f, typeof(IgnoreMemberAttribute))).ToList();
            }

            return this.fields;
        }
        
        public override int GetHashCode()
        {
            unchecked   //allow overflow
            {
                List<object> values = new List<object>();

                values.AddRange(GetProperties().Select(prop => prop.GetValue(this, null)));
                values.AddRange(GetFields().Select(field => field.GetValue(this)));

                return HashCodeHelper.GetHashCode(values);

            }
        }
    }
}
