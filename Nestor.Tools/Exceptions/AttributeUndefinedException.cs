namespace Nestor.Tools.Exceptions
{
    public class AttributeUndefinedException : NestorException
    {
        public AttributeUndefinedException(string className, string propName) : base($"Aucune propriété de l'objet {className} ne comporte d'attribut {propName}")
        {
        }
    }
}
