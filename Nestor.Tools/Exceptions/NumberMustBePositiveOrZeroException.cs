namespace Nestor.Tools.Exceptions
{
    public class NumberMustBePositiveOrZeroException : NestorException
    {
        public NumberMustBePositiveOrZeroException() : base("{0} must be greater than or equal to zero")
        {
        }
    }
}