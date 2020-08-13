namespace Nestor.Tools.Exceptions
{
    public class NegativeLifeTimeException : NestorException
    {
        public NegativeLifeTimeException() : base("The life-time must be strictly positive")
        {
        }
    }
}