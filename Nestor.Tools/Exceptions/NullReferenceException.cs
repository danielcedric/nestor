using System;

namespace Nestor.Tools.Exceptions
{
    public class NullReferenceException<T> : NullReferenceException where T : class
    {
        public NullReferenceException() : base($"The object of type {typeof(T).FullName} is null")
        {

        }

        public NullReferenceException(string message) : base(message)
        {

        }
    }
}
