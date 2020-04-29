using Nestor.Tools.Exceptions;

namespace Nestor.Tools.Infrastructure.Exceptions
{
    public class DuplicateAliasException : NestorException
    {
        public DuplicateAliasException(string message) : base(message)
        {
        }
    }
}
