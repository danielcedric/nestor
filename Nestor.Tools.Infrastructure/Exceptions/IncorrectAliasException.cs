using Nestor.Tools.Exceptions;

namespace Nestor.Tools.Infrastructure.Exceptions
{
    public class IncorrectAliasException : NestorException
    {
        public IncorrectAliasException(string message) : base(message)
        {
        }
    }
}