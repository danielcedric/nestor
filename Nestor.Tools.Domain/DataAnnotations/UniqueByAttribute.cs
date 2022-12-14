using System;
using System.Collections.Generic;

namespace Nestor.Tools.Domain.DataAnnotations
{
    /// <summary>
    ///     Attribute qui placé sur une propriété d'un objet modèle, indique que celle-ci sera mappée comme composant la clé
    ///     unique de l'objet
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class UniqueByAttribute : Attribute
    {
        public UniqueByAttribute(params string[] hashCodeProps)
        {
            PropertyNames = hashCodeProps;
        }

        public IEnumerable<string> PropertyNames { get; }
    }
}