namespace Nestor.Tools.Exceptions
{
    public class ExpressionMustBeMethodCallException : NestorException
    {
        public ExpressionMustBeMethodCallException() : base("The expression must be a method call")
        {
        }
    }
}