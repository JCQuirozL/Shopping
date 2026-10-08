namespace Shopping.Helpers
{
    public interface ICurrencyHelper
    {
        /// <summary>
        /// Returns the exchange rates relative to the base currency (MXN = 1).
        /// </summary>
        Task<Dictionary<string, decimal>> GetRatesAsync();

        /// <summary>
        /// Converts an amount expressed in the base currency (MXN) to the given target currency code.
        /// </summary>
        Task<decimal> ConvertFromBaseAsync(decimal amountInBaseCurrency, string targetCurrencyCode);

        string GetSymbol(string currencyCode);

        IEnumerable<string> SupportedCurrencies { get; }
    }
}
