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
    public class OrdersController : Controller
    {
        private readonly DataContext _context;

        public OrdersController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(OrderStatus? status)
        {
            IQueryable<Sale> query = _context.Sales
                .Include(s => s.User)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product);

            if (status.HasValue)
            {
                query = query.Where(s => s.OrderStatus == status);
            }

            ViewBag.Status = status;
            return View(await query.OrderByDescending(s => s.Date).ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            Sale sale = await _context.Sales
                .Include(s => s.User)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .Include(s => s.StatusHistories)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null)
            {
                return NotFound();
            }

            sale.StatusHistories = sale.StatusHistories.OrderBy(h => h.Date).ToList();
            return View(sale);
        }

        public async Task<IActionResult> ChangeStatus(int id)
        {
            Sale sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            ChangeOrderStatusViewModel model = new()
            {
                SaleId = sale.Id,
                OrderStatus = sale.OrderStatus,
                TrackingNumber = sale.TrackingNumber,
                Carrier = sale.Carrier,
                EstimatedDeliveryDate = sale.EstimatedDeliveryDate,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(ChangeOrderStatusViewModel model)
        {
            Sale sale = await _context.Sales
                .Include(s => s.StatusHistories)
                .FirstOrDefaultAsync(s => s.Id == model.SaleId);

            if (sale == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            sale.OrderStatus = model.OrderStatus;
            sale.TrackingNumber = model.TrackingNumber;
            sale.Carrier = model.Carrier;
            sale.EstimatedDeliveryDate = model.EstimatedDeliveryDate;

            sale.StatusHistories ??= new List<OrderStatusHistory>();
            sale.StatusHistories.Add(new OrderStatusHistory
            {
                OrderStatus = model.OrderStatus,
                Date = DateTime.UtcNow,
                Notes = model.Notes,
            });

            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = sale.Id });
        }
    }
}
