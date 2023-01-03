using System;
namespace Nestor.Tools.Exceptions
{
    public class NoUniqueByAttributeForTypeException<T> : NestorException
    {
        public NoUniqueByAttributeForTypeException() : base($"The entity type {typeof(T).Name} has no UniqueBy attribute, so Repository.GetByUniqueKey({typeof(T).Name}) function was not available.")
        {

        }
    }
}

