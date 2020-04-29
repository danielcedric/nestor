namespace Nestor.Tools.Exceptions
{
    public class InvalidCastFromToException<TFRom, TTo> : NestorException
    {
        public InvalidCastFromToException(TFRom from, TTo to) : base($"Invalid cast from '{from.GetType().Name}' to '{to.GetType().Name}'")
        {
        }
    }
}
