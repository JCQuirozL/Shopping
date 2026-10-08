using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;

namespace Shopping.ViewComponents
{
    public class CartBadgeViewComponent : ViewComponent
    {
        private readonly DataContext _context;

        public CartBadgeViewComponent(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            float quantity = 0f;

            if (User?.Identity?.IsAuthenticated == true && !string.IsNullOrEmpty(User.Identity.Name))
            {
                quantity = await _context.TemporalSales
                    .Where(ts => ts.User.Email == User.Identity.Name)
                    .SumAsync(ts => (float?)ts.Quantity) ?? 0f;
            }

            return View(quantity);
        }
    }
}
