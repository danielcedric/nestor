namespace Nestor.Tools.Exceptions
{
    public class MaxLengthCantBeLessThanLengthOfEllipsisStringException : NestorException
    {
        public MaxLengthCantBeLessThanLengthOfEllipsisStringException() : base("maxLength can not be less than the length of ellipsisString")
        {
        }
    }
}
