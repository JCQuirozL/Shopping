using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Shopping.Data;
using Shopping.Data.Entities;
using System.IO;
using System.Text.Json;

namespace Shopping.Helpers
{
    public class CurrencyHelper : ICurrencyHelper
    {
        private const string BaseCurrency = "MXN";
        private const string CacheKey = "ExchangeRates";
        private static readonly string[] _supportedCurrencies = { "MXN", "USD", "BRL" };

        private readonly HttpClient _httpClient;
        private readonly DataContext _context;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CurrencyHelper> _logger;

        public CurrencyHelper(HttpClient httpClient, DataContext context, IMemoryCache cache, ILogger<CurrencyHelper> logger)
        {
            _httpClient = httpClient;
            _context = context;
            _cache = cache;
            _logger = logger;
        }

        public IEnumerable<string> SupportedCurrencies => _supportedCurrencies;

        public string GetSymbol(string currencyCode)
        {
            return currencyCode switch
            {
                "USD" => "US$",
                "BRL" => "R$",
                _ => "$",
            };
        }

        public async Task<Dictionary<string, decimal>> GetRatesAsync()
        {
            if (_cache.TryGetValue(CacheKey, out Dictionary<string, decimal> cached))
            {
                return cached;
            }

            Dictionary<string, decimal> rates = await FetchRatesFromApiAsync();

            if (rates == null || rates.Count == 0)
            {
                rates = await LoadRatesFromDatabaseAsync();
            }
            else
            {
                await SaveRatesToDatabaseAsync(rates);
            }

            if (rates == null || rates.Count == 0)
            {
                // Last resort fallback so the application keeps working offline.
                rates = new Dictionary<string, decimal>
                {
                    ["MXN"] = 1m,
                    ["USD"] = 0.059m,
                    ["BRL"] = 0.30m,
                };
            }

            if (!rates.ContainsKey(BaseCurrency))
            {
                rates[BaseCurrency] = 1m;
            }

            _cache.Set(CacheKey, rates, TimeSpan.FromMinutes(60));
            return rates;
        }

        public async Task<decimal> ConvertFromBaseAsync(decimal amountInBaseCurrency, string targetCurrencyCode)
        {
            if (string.IsNullOrWhiteSpace(targetCurrencyCode) || targetCurrencyCode == BaseCurrency)
            {
                return amountInBaseCurrency;
            }

            try
            {
                Dictionary<string, decimal> rates = await GetRatesAsync();
                if (!rates.TryGetValue(targetCurrencyCode, out decimal rate))
                {
                    return amountInBaseCurrency;
                }

                return Math.Round(amountInBaseCurrency * rate, 2);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "No se pudo convertir el monto a {Currency}, se muestra el valor sin convertir.", targetCurrencyCode);
                return amountInBaseCurrency;
            }
        }

        private async Task<Dictionary<string, decimal>> FetchRatesFromApiAsync()
        {
            try
            {
                // Free, no-API-key endpoint. Base currency is MXN so rates are directly usable.
                using HttpResponseMessage httpResponse = await _httpClient.GetAsync($"https://open.er-api.com/v6/latest/{BaseCurrency}");
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return null;
                }

                using Stream stream = await httpResponse.Content.ReadAsStreamAsync();
                using JsonDocument document = await JsonDocument.ParseAsync(stream);
                JsonElement root = document.RootElement;

                if (!root.TryGetProperty("result", out JsonElement resultElement) ||
                    resultElement.GetString() != "success")
                {
                    return null;
                }

                if (!root.TryGetProperty("rates", out JsonElement ratesElement))
                {
                    return null;
                }

                Dictionary<string, decimal> rates = new();
                foreach (string code in _supportedCurrencies)
                {
                    if (ratesElement.TryGetProperty(code, out JsonElement rateElement) &&
                        rateElement.TryGetDecimal(out decimal rateValue))
                    {
                        rates[code] = rateValue;
                    }
                }

                return rates;
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "No se pudo obtener el tipo de cambio desde el servicio externo, se usará el último valor conocido.");
                return null;
            }
        }

        private async Task<Dictionary<string, decimal>> LoadRatesFromDatabaseAsync()
        {
            try
            {
                List<ExchangeRate> stored = await _context.ExchangeRates.ToListAsync();
                return stored.ToDictionary(r => r.CurrencyCode, r => r.Rate);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "No se pudo leer el tipo de cambio desde la base de datos.");
                return new Dictionary<string, decimal>();
            }
        }

        private async Task SaveRatesToDatabaseAsync(Dictionary<string, decimal> rates)
        {
            try
            {
                foreach (KeyValuePair<string, decimal> rate in rates)
                {
                    ExchangeRate existing = await _context.ExchangeRates
                        .FirstOrDefaultAsync(r => r.CurrencyCode == rate.Key);

                    if (existing == null)
                    {
                        _context.ExchangeRates.Add(new ExchangeRate
                        {
                            CurrencyCode = rate.Key,
                            Rate = rate.Value,
                            LastUpdated = DateTime.UtcNow,
                        });
                    }
                    else
                    {
                        existing.Rate = rate.Value;
                        existing.LastUpdated = DateTime.UtcNow;
                        _context.ExchangeRates.Update(existing);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "No se pudo guardar el tipo de cambio en la base de datos.");
            }
        }
    }
}
