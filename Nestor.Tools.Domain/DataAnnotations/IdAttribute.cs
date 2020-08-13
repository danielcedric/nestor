using System;

namespace Nestor.Tools.Domain.DataAnnotations
{
    /// <summary>
    ///     Attribute qui placé sur une propriété d'un objet modèle, indique que celle-ci sera mappée comme identifiant
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IdAttribute : Attribute
    {
    }
}