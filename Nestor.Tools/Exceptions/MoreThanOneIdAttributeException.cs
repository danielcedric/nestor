namespace Nestor.Tools.Exceptions
{
    public class MoreThanOneIdAttributeException : NestorException
    {
        public MoreThanOneIdAttributeException() : base("The Id attribute is set several times for the object {0}")
        {
        }
    }
}