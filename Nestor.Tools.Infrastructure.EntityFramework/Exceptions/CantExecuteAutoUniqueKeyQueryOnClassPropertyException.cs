namespace Nestor.Tools.Infrastructure.EntityFramework.Exceptions
{
    /// <summary>
    ///     Exception déclenchée par la fonction GetByUniqueKey si une des propriétés [UniqueKey] est placée sur une propriété
    ///     qui est une instance de classe
    /// </summary>
    public class CantExecuteAutoUniqueKeyQueryOnClassPropertyException : EntityFrameworkException
    {
        public CantExecuteAutoUniqueKeyQueryOnClassPropertyException(string propertyName) : base(
            $"Unable to apply an automatic query of type 'GetByUniqueKey' on '{propertyName}' because this is not a primive type")
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; set; }
    }
}