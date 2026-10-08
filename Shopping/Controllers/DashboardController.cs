using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Data.Entities;
using Shopping.Enums;
using Shopping.Models;

namespace Shopping.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private const int LowStockThreshold = 10;

        private readonly DataContext _context;

        public DashboardController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Sale> sales = await _context.Sales
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .Include(s => s.User)
                .Where(s => s.OrderStatus != OrderStatus.Cancelado)
                .ToListAsync();

            List<Product> products = await _context.Products.ToListAsync();

            DashboardViewModel model = new()
            {
                TotalOrders = sales.Count,
                TotalRevenue = sales.Sum(s => s.Value),
                TotalCustomers = await _context.Users.CountAsync(),
                TotalProducts = products.Count,
                LowStockCount = products.Count(p => p.Stock <= LowStockThreshold),
                LowStockProducts = products
                    .Where(p => p.Stock <= LowStockThreshold)
                    .OrderBy(p => p.Stock)
                    .Take(10)
                    .ToList(),
                RecentOrders = sales
                    .OrderByDescending(s => s.Date)
                    .Take(10)
                    .ToList(),
            };

            DateTime startDate = DateTime.UtcNow.Date.AddDays(-29);
            for (DateTime day = startDate; day <= DateTime.UtcNow.Date; day = day.AddDays(1))
            {
                model.SalesDays.Add(day.ToString("dd/MM"));
                model.SalesAmounts.Add(sales
                    .Where(s => s.Date.Date == day)
                    .Sum(s => s.Value));
            }

            foreach (OrderStatus status in Enum.GetValues(typeof(OrderStatus)))
            {
                int count = await _context.Sales.CountAsync(s => s.OrderStatus == status);
                model.OrderStatusLabels.Add(status.ToString());
                model.OrderStatusCounts.Add(count);
            }

            var topProducts = sales
                .SelectMany(s => s.SaleDetails)
                .GroupBy(sd => sd.Product.Name)
                .Select(g => new { Name = g.Key, Quantity = g.Sum(sd => sd.Quantity) })
                .OrderByDescending(g => g.Quantity)
                .Take(5)
                .ToList();

            model.TopProductNames = topProducts.Select(p => p.Name).ToList();
            model.TopProductQuantities = topProducts.Select(p => p.Quantity).ToList();

            return View(model);
        }
    }
}
