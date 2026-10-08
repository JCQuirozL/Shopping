using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Shopping.Controllers
{
    public class CurrencyController : Controller
    {
        private static readonly Dictionary<string, (string Culture, string Currency)> Regions = new()
        {
            ["MX"] = ("es-MX", "MXN"),
            ["BR"] = ("pt-BR", "BRL"),
            ["US"] = ("en-US", "USD"),
        };

        [HttpGet]
        public IActionResult SetRegion(string region, string returnUrl)
        {
            if (Regions.TryGetValue(region, out (string Culture, string Currency) config))
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(config.Culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true });

                Response.Cookies.Append("Currency", config.Currency, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                });

                Response.Cookies.Append("Region", region, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                });
            }

            if (Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
