using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nestor.Tools.Domain.Entities
{
    public class Enumeration : IComparable
    {
        #region Properties

        /// <summary>
        ///     Affecte ou obtient le nom de l'énumération
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Affecte ou obtient l'identifiant de l'énumération
        /// </summary>
        public int Id { get; set; }

        #endregion

        #region Constructors

        protected Enumeration()
        {
        }

        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Obtient toutes les instances de l'énumération
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetAll<T>() where T : Enumeration, new()
        {
            var type = typeof(T);
            var fields = type.GetTypeInfo()
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (var info in fields)
            {
                var instance = new T();
                var locatedValue = info.GetValue(instance) as T;

                if (locatedValue != null)
                    yield return locatedValue;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;

            if (otherValue == null)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }


        public int CompareTo(object obj)
        {
            return Id.CompareTo(((Enumeration) obj)?.Id);
        }

        #endregion
    }
}