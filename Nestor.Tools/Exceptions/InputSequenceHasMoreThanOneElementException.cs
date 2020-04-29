namespace Nestor.Tools.Exceptions
{
    public class InputSequenceHasMoreThanOneElementException : NestorException
    {
        public InputSequenceHasMoreThanOneElementException() : base("The input sequence contains several elements")
        {
        }
    }
}
