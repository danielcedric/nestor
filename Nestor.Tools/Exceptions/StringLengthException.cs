namespace Nestor.Tools.Exceptions
{
    public class StringLengthException : NestorException
    {
        public StringLengthException() : base("The value cannot exceed {0} characters")
        {
        }
    }
}