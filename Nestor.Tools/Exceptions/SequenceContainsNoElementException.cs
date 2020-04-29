namespace Nestor.Tools.Exceptions
{
    public class SequenceContainsNoElementException : NestorException
    {
        public SequenceContainsNoElementException() : base("The sequence contains no elements")
        {
        }
    }
}
