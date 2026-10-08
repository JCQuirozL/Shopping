using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Data.Entities;
using Shopping.Helpers;

namespace Shopping.Controllers
{
    [Authorize]
    public class MyOrdersController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public MyOrdersController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task<IActionResult> Index()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            List<Sale> orders = await _context.Sales
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .Where(s => s.User.Id == user.Id)
                .OrderByDescending(s => s.Date)
                .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> Track(int id)
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            Sale sale = await _context.Sales
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .Include(s => s.StatusHistories)
                .FirstOrDefaultAsync(s => s.Id == id && s.User.Id == user.Id);

            if (sale == null)
            {
                return NotFound();
            }

            sale.StatusHistories = sale.StatusHistories.OrderBy(h => h.Date).ToList();
            return View(sale);
        }
    }
}
