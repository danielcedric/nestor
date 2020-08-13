namespace Nestor.Tools.Exceptions
{
    public class NoMatchFoundException : NestorException
    {
        public NoMatchFoundException() : base("No matching case was found")
        {
        }
    }
}