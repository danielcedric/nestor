namespace Nestor.Tools.Exceptions
{
    public class ArrayCapacityInsufficientException : NestorException
    {
        public ArrayCapacityInsufficientException() : base(
            "The destination table is not long enough. Check destIndex and length, and the lower limits of the table.")
        {
        }
    }
}