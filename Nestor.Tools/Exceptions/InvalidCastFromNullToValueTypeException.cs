namespace Nestor.Tools.Exceptions
{
    public class InvalidCastFromNullToValueTypeException : NestorException
    {
        public InvalidCastFromNullToValueTypeException() : base("Can not convert a null object to a value type")
        {
        }
    }
}
