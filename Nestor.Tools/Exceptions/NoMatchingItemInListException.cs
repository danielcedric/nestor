namespace Nestor.Tools.Exceptions
{
    public class NoMatchingItemInListException : NestorException
    {
        public NoMatchingItemInListException() : base("The list contains no matching items")
        {
        }
    }
}