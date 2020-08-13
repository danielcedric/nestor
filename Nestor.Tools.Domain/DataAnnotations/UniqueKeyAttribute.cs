using System;

namespace Nestor.Tools.Domain.DataAnnotations
{
    /// <summary>
    ///     Attribute qui placé sur une propriété d'un objet modèle, indique que celle-ci sera mappée comme composant la clé
    ///     unique de l'objet
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class UniqueKeyAttribute : Attribute
    {
    }
}