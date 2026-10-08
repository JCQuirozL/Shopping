using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Data.Entities;
using Shopping.Models;

namespace Shopping.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InventoryController : Controller
    {
        private const int LowStockThreshold = 10;

        private readonly DataContext _context;

        public InventoryController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search, bool lowStockOnly = false)
        {
            IQueryable<Product> query = _context.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Name.Contains(search));
            }

            if (lowStockOnly)
            {
                query = query.Where(p => p.Stock <= LowStockThreshold);
            }

            ViewBag.Search = search;
            ViewBag.LowStockOnly = lowStockOnly;
            ViewBag.LowStockThreshold = LowStockThreshold;

            return View(await query.OrderBy(p => p.Name).ToListAsync());
        }

        public async Task<IActionResult> Movements(int id)
        {
            Product product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Product = product;

            List<InventoryMovement> movements = await _context.InventoryMovements
                .Include(m => m.Product)
                .Where(m => m.Product.Id == id)
                .OrderByDescending(m => m.Date)
                .ToListAsync();

            return View(movements);
        }

        public async Task<IActionResult> AdjustStock(int id)
        {
            Product product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            InventoryAdjustmentViewModel model = new()
            {
                ProductId = product.Id,
                ProductName = product.Name,
                CurrentStock = product.Stock,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdjustStock(InventoryAdjustmentViewModel model)
        {
            Product product = await _context.Products.FindAsync(model.ProductId);
            if (product == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                model.ProductName = product.Name;
                model.CurrentStock = product.Stock;
                return View(model);
            }

            float change = model.MovementType == "Salida" ? -model.Quantity : model.Quantity;

            if (product.Stock + change < 0)
            {
                ModelState.AddModelError(string.Empty, "La existencia no puede quedar negativa.");
                model.ProductName = product.Name;
                model.CurrentStock = product.Stock;
                return View(model);
            }

            product.Stock += change;
            _context.Products.Update(product);

            _context.InventoryMovements.Add(new InventoryMovement
            {
                Product = product,
                Date = DateTime.UtcNow,
                QuantityChange = change,
                ResultingStock = product.Stock,
                Reason = model.Reason,
                UserName = User.Identity.Name,
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Movements), new { id = product.Id });
        }
    }
}
