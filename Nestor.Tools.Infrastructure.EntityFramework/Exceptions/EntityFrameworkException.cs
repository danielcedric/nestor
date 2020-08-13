using System;
using Nestor.Tools.Exceptions;

namespace Nestor.Tools.Infrastructure.EntityFramework.Exceptions
{
    public class EntityFrameworkException : NestorException
    {
        public EntityFrameworkException()
        {
        }

        public EntityFrameworkException(string message) : base(message)
        {
        }

        public EntityFrameworkException(string message, Exception e) : base(message, e)
        {
        }

        public EntityFrameworkException(Exception e) : base(e.Message, e)
        {
        }

        public EntityFrameworkException(Exception e, object parameter) : base(e.Message, e)
        {
            Parameter = parameter;
        }

        #region Properties

        /// <summary>
        ///     Le paramètre de l'appel de méthode qui a déclenché l'excetion
        /// </summary>
        public object Parameter { get; set; }

        #endregion
    }
}