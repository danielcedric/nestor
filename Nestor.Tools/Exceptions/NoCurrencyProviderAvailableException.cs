namespace Nestor.Tools.Exceptions
{
    public class NoCurrencyProviderAvailableException : NestorException
    {
        public NoCurrencyProviderAvailableException(string cultureCode) : base(
            $"No currency provider available for culture '{cultureCode}'")
        {
        }
    }
}