namespace Nestor.Tools.Exceptions
{
    public class RomanMaxValueException : NestorException
    {
        public RomanMaxValueException() : base("A number in Roman numerals can not be greater than {0}")
        {
        }
    }
}