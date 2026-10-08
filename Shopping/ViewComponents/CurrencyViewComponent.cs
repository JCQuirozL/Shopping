using Microsoft.AspNetCore.Mvc;
using Shopping.Helpers;

namespace Shopping.ViewComponents
{
    public class CurrencyViewComponent : ViewComponent
    {
        private readonly ICurrencyHelper _currencyHelper;
        private readonly ILogger<CurrencyViewComponent> _logger;

        public CurrencyViewComponent(ICurrencyHelper currencyHelper, ILogger<CurrencyViewComponent> logger)
        {
            _currencyHelper = currencyHelper;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync(decimal amount)
        {
            string currencyCode = Request.Cookies["Currency"] ?? "MXN";

            try
            {
                decimal converted = await _currencyHelper.ConvertFromBaseAsync(amount, currencyCode);
                string symbol = _currencyHelper.GetSymbol(currencyCode);
                return Content($"{symbol}{converted:N2} {currencyCode}");
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "No se pudo mostrar el precio convertido, se muestra el monto original.");
                return Content($"${amount:N2} MXN");
            }
        }
    }
}
