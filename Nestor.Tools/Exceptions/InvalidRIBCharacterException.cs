namespace Nestor.Tools.Exceptions
{
    public class InvalidRIBCharacterException : NestorException
    {
        public InvalidRIBCharacterException() : base("The character to be converted must be an uppercase letter")
        {
        }
    }
}