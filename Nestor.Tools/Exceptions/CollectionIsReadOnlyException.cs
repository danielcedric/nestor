namespace Nestor.Tools.Exceptions
{
    public class CollectionIsReadOnlyException : NestorException
    {
        public CollectionIsReadOnlyException(): base("The collection is read-only")
        {
        }
    }
}
