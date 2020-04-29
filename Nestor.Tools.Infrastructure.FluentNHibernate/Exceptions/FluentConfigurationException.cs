using Nestor.Tools.Exceptions;
using System;

namespace Nestor.Tools.Infrastructure.FluentNHibernate.Exceptions
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
