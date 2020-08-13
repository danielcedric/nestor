namespace Nestor.Tools.Exceptions
{
    public class MaxThreadCountOutOfRangeException : NestorException
    {
        public MaxThreadCountOutOfRangeException() : base(
            "The maximum number of threads must be greater than or equal to 1")
        {
        }
    }
}