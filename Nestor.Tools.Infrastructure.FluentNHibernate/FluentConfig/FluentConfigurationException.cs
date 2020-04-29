using Nestor.Tools.Exceptions;
using System;

namespace Nestor.Tools.Infrastructure.FluentConfig
{
    public class FluentConfigurationException : NestorException
    {
        public FluentConfigurationException()
        {

        }

        public FluentConfigurationException(string message) : base(message)
        {

        }

        public FluentConfigurationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
