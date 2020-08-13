namespace Nestor.Tools.Exceptions
{
    public class MaxLengthCantBeLessThanException : NestorException
    {
        public MaxLengthCantBeLessThanException() : base($"maxLength can not be less than {0}")
        {
        }
    }
}