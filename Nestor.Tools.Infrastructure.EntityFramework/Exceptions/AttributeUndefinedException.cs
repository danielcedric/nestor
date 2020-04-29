using Nestor.Tools.Exceptions;
using System;

namespace Nestor.Tools.Infrastructure.EntityFramework.Exceptions
{
    /// <summary>
    /// Exception déclenché si la recherche de l'attribut n'a pas fonctionné
    /// </summary>
    public class AttributeUndefinedException : NestorException
    {
        #region Properties
        /// <summary>
        /// Affecte ou obtient le type de l'attribut concerné par l'exception
        /// </summary>
        public Type Attribute { get; set; }
        /// <summary>
        /// Affecte ou obtient le type de l'objet qui a déclenché l'erreur
        /// </summary>
        public Type ObjectType { get; set; }
        #endregion

        #region Constructors
        public AttributeUndefinedException(Type attributeType, Type objectType) : base($"No attribute of type {attributeType.Name} was defined for the type {objectType.Name}")
        {
            this.Attribute = attributeType;
            this.ObjectType = objectType;
        }
        #endregion
    }

    /// <summary>
    /// Exception déclenché si la recherche de l'attribut n'a pas fonctionné
    /// </summary>
    public class AttributeUndefinedException<TAttribute, TObject> : AttributeUndefinedException
        where TAttribute : Attribute
        where TObject : class
    {
        #region Properties
        
        #endregion

        #region Constructors
        public AttributeUndefinedException() : base(typeof(TAttribute), typeof(TObject))
        {
        }
        #endregion
    }
}
