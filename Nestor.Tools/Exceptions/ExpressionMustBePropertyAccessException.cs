namespace Nestor.Tools.Exceptions
{
    public class ExpressionMustBePropertyAccessException : NestorException
    {
        public ExpressionMustBePropertyAccessException() : base("The expression must be access to a property")
        {
        }
    }
}