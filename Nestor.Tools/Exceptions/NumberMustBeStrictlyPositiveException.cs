namespace Nestor.Tools.Exceptions
{
    public class NumberMustBeStrictlyPositiveException : NestorException
    {
        public NumberMustBeStrictlyPositiveException() : base("{0} must be strictly greater than zero")
        {
        }
    }
}