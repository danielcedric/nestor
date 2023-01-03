using System;
namespace Nestor.Tools.Exceptions
{
    public class UniqueByAttributeIsEmptyException<T> : NestorException
    {
        public UniqueByAttributeIsEmptyException() : base($"The entity type {typeof(T).Name} has has a UniqueBy attribute, but its configuration is empty")
        {

        }
    }
}

