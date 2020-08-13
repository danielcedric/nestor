namespace Nestor.Tools.Exceptions
{
    public class InvalidNirException : NestorException
    {
        public InvalidNirException() : base(
            "The NIR contains invalid characters. It must be composed of 13 digits except the department which can be 2A or 2B")
        {
        }
    }
}