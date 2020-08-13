namespace Nestor.Tools.Exceptions
{
    public class RomanMinValueException : NestorException
    {
        public RomanMinValueException() : base("A number in Roman numerals can not be less than {0}")
        {
        }
    }
}