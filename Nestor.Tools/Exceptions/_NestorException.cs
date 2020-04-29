using System;

namespace Nestor.Tools.Exceptions
{
    [Serializable]
    public class NestorException : ApplicationException
    {

        public NestorException()
        {

        }

        public NestorException(string message) : base(message)
        {

        }

        public NestorException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
