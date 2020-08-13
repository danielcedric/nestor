namespace Nestor.Tools.Exceptions
{
    public class DictionaryIsReadOnlyException : NestorException
    {
        public DictionaryIsReadOnlyException() : base("Dictionary is read-only")
        {
        }
    }
}