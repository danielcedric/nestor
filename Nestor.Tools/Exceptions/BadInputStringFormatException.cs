namespace Nestor.Tools.Exceptions
{
    public class BadInputStringFormatException : NestorException
    {
        public BadInputStringFormatException() : base("The input string format is incorrect")
        {
        }
    }
}