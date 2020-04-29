namespace Nestor.Tools.Exceptions
{
    public class IniFileNameNotSetException : NestorException
    {
        public IniFileNameNotSetException() : base("Le nom du fichier n'est pas défini")
        {
        }
    }
}
