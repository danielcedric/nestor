namespace Nestor.Tools.Exceptions
{
    public class AllStreamsMustBeWritableException : NestorException
    {
        public AllStreamsMustBeWritableException() : base("All streams must be writable")
        {
        }
    }
}
