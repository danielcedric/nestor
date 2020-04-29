using Nestor.Tools.Exceptions;

namespace Nestor.Tools.Infrastructure.EntityFramework.Exceptions
{
    public class EntityFrameworkException : NestorException
    {
        #region Properties
        /// <summary>
        /// Le paramètre de l'appel de méthode qui a déclenché l'excetion
        /// </summary>
        public object Parameter { get; set; }
        #endregion

        public EntityFrameworkException() : base() { }
        public EntityFrameworkException(string message) : base(message) { }
        public EntityFrameworkException(string message, System.Exception e) : base(message, e) { }
        public EntityFrameworkException(System.Exception e) : base(e.Message, e) { }
        public EntityFrameworkException(System.Exception e, object parameter) : base(e.Message, e) { Parameter = parameter; }
    }
}
