using Nestor.Tools.Exceptions;

namespace Nestor.Tools.Infrastructure.EntityFramework.Exceptions
{
    public class OutsideTransactionalContext : NestorException
    {
        public OutsideTransactionalContext() : base("You must be in transactional context")
        {
        }
    }
}