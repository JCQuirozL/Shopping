using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Data.Entities;
using Shopping.Models;

namespace Shopping.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomersController : Controller
    {
        private readonly DataContext _context;

        public CustomersController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search)
        {
            IQueryable<User> query = _context.Users
                .Include(u => u.City)
                .ThenInclude(c => c.State)
                .ThenInclude(s => s.Country);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    u.FirstName.Contains(search) ||
                    u.LastName.Contains(search) ||
                    u.Email.Contains(search) ||
                    u.Document.Contains(search));
            }

            ViewBag.Search = search;
            return View(await query.OrderBy(u => u.FirstName).ToListAsync());
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            User user = await _context.Users
                .Include(u => u.City)
                .ThenInclude(c => c.State)
                .ThenInclude(s => s.Country)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            List<Address> addresses = await _context.Addresses
                .Include(a => a.City)
                .Where(a => a.User.Id == id)
                .ToListAsync();

            List<Sale> orders = await _context.Sales
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .Where(s => s.User.Id == id)
                .OrderByDescending(s => s.Date)
                .ToListAsync();

            CustomerDetailsViewModel model = new()
            {
                User = user,
                Addresses = addresses,
                Orders = orders,
                TotalSpent = orders.Sum(o => o.Value),
            };

            return View(model);
        }
    }
}
