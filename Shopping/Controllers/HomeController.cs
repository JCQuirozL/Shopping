using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Common;
using Shopping.Data;
using Shopping.Data.Entities;
using Shopping.Helpers;
using Shopping.Models;
using System.Diagnostics;

namespace Shopping.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IOrdersHelper _ordersHelper;


        public HomeController(ILogger<HomeController> logger, DataContext context, IUserHelper userHelper, IOrdersHelper ordersHelper)
        {
            _logger = logger;
            _context = context;
            _userHelper = userHelper;
            _ordersHelper = ordersHelper;
        }

        [Authorize]
        public IActionResult OrderSuccess()
        {
            ViewBag.PaymentMethod = TempData["PaymentMethod"];
            ViewBag.PaymentTotal = TempData["PaymentTotal"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowCart(ShowCartViewModel model)
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }

            List<TemporalSale> temporalSales = await _context.TemporalSales
                .Include(ts => ts.Product)
                .Where(ts => ts.User.Id == user.Id)
                .ToListAsync();

            if (!temporalSales.Any())
            {
                return RedirectToAction(nameof(ShowCart));
            }

            TempData["CartRemarks"] = model.Remarks;
            TempData["ShippingRecipient"] = model.ShippingRecipient;
            TempData["ShippingPhone"] = model.ShippingPhone;
            TempData["ShippingAddress"] = model.ShippingAddress;
            return RedirectToAction(nameof(Payment));
        }

        [Authorize]
        public async Task<IActionResult> Payment()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            List<TemporalSale> temporalSales = await _context.TemporalSales
                .Include(ts => ts.Product)
                .Where(ts => ts.User.Id == user.Id)
                .ToListAsync();

            if (!temporalSales.Any())
            {
                return RedirectToAction(nameof(ShowCart));
            }

            Address defaultAddress = await _context.Addresses
                .Include(a => a.City)
                .Where(a => a.User.Id == user.Id)
                .OrderByDescending(a => a.IsDefault)
                .FirstOrDefaultAsync();

            PaymentViewModel model = new()
            {
                Items = temporalSales.Sum(ts => ts.Quantity),
                Total = temporalSales.Sum(ts => ts.Value),
                ShippingRecipient = TempData.Peek("ShippingRecipient") as string ?? user.FullName,
                ShippingPhone = TempData.Peek("ShippingPhone") as string ?? user.PhoneNumber,
                ShippingAddress = TempData.Peek("ShippingAddress") as string ??
                    (defaultAddress != null ? defaultAddress.FullAddress : $"{user.Address}, {user.City?.Name}"),
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(PaymentViewModel model)
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            List<TemporalSale> temporalSales = await _context.TemporalSales
                .Include(ts => ts.Product)
                .ThenInclude(p => p.ProductImages)
                .Where(ts => ts.User.Id == user.Id)
                .ToListAsync();

            if (!temporalSales.Any())
            {
                return RedirectToAction(nameof(ShowCart));
            }

            model.Items = temporalSales.Sum(ts => ts.Quantity);
            model.Total = temporalSales.Sum(ts => ts.Value);

            bool requiresCard = model.PaymentMethod is "CreditCard" or "DebitCard";
            if (requiresCard)
            {
                string digits = new string((model.CardNumber ?? string.Empty).Where(char.IsDigit).ToArray());

                if (digits.Length < 13
                    || string.IsNullOrWhiteSpace(model.CardHolder)
                    || string.IsNullOrWhiteSpace(model.ExpirationDate)
                    || string.IsNullOrWhiteSpace(model.Cvv))
                {
                    ModelState.AddModelError(string.Empty, "Revisa los datos de la tarjeta, hay campos incompletos o inválidos.");
                    return View(model);
                }

                if (digits.EndsWith("0000"))
                {
                    ModelState.AddModelError(string.Empty, "Pago rechazado por el banco emisor: fondos insuficientes. Intenta con otra tarjeta.");
                    return View(model);
                }
            }

            ShowCartViewModel cartModel = new()
            {
                User = user,
                Remarks = TempData["CartRemarks"] as string,
                TemporalSales = temporalSales,
                ShippingRecipient = model.ShippingRecipient,
                ShippingPhone = model.ShippingPhone,
                ShippingAddress = model.ShippingAddress,
            };

            Response response = await _ordersHelper.ProcessOrderAsync(cartModel);
            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                return View(model);
            }

            TempData["PaymentMethod"] = model.PaymentMethod;
            TempData["PaymentTotal"] = model.Total.ToString("C2");
            return RedirectToAction(nameof(OrderSuccess));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            TemporalSale temporalSale = await _context.TemporalSales.FindAsync(id);
            if (temporalSale == null)
            {
                return NotFound();
            }

            EditTemporalSaleViewModel model = new()
            {
                Id = temporalSale.Id,
                Quantity = temporalSale.Quantity,
                Remarks = temporalSale.Remarks,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditTemporalSaleViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    TemporalSale temporalSale = await _context.TemporalSales.FindAsync(id);
                    temporalSale.Quantity = model.Quantity;
                    temporalSale.Remarks = model.Remarks;
                    _context.Update(temporalSale);
                    await _context.SaveChangesAsync();
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                    return View(model);
                }
                return RedirectToAction(nameof(ShowCart));
            }
            return View(model);
        }

        public async Task<IActionResult> DecreaseQuantity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            TemporalSale temporalSale = await _context.TemporalSales.FindAsync(id);
            if (temporalSale == null)
            {
                return NotFound();
            }
            if (temporalSale.Quantity > 1)
            {
                temporalSale.Quantity--;
                _context.TemporalSales.Update(temporalSale);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ShowCart));
        }

        public async Task<IActionResult> IncreaseQuantity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            TemporalSale temporalSale = await _context.TemporalSales.FindAsync(id);
            if (temporalSale == null)
            {
                return NotFound();
            }
            temporalSale.Quantity++;
            _context.TemporalSales.Update(temporalSale);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowCart));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            TemporalSale temporalSale = await _context.TemporalSales.FindAsync(id);
            if (temporalSale == null)
            {
                return NotFound();
            }
            _context.TemporalSales.Remove(temporalSale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowCart));
        }

        [Authorize]
        public async Task<IActionResult> ShowCart()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }
            List<TemporalSale>? temporalSales = await _context.TemporalSales
            .Include(ts => ts.Product)
            .ThenInclude(p => p.ProductImages)
            .Where(ts => ts.User.Id == user.Id)
            .ToListAsync();

            List<Address> savedAddresses = await _context.Addresses
                .Include(a => a.City)
                .Where(a => a.User.Id == user.Id)
                .ToListAsync();

            Address defaultAddress = savedAddresses.FirstOrDefault(a => a.IsDefault) ?? savedAddresses.FirstOrDefault();

            ShowCartViewModel model = new()
            {
                User = user,
                TemporalSales = temporalSales,
                SavedAddresses = savedAddresses,
                ShippingRecipient = user.FullName,
                ShippingPhone = user.PhoneNumber,
                ShippingAddress = defaultAddress != null ? defaultAddress.FullAddress : $"{user.Address}, {user.City?.Name}",
            };
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product product = await _context.Products
            .Include(p => p.ProductImages)
            .Include(p => p.ProductCategories)
            .ThenInclude(pc => pc.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            string categories = string.Empty;
            foreach (ProductCategory? category in product.ProductCategories)
            {
                categories += $"{category.Category.Name}, ";
            }
            categories = categories.Substring(0, categories.Length - 2);
            AddProductToCartViewModel model = new()
            {
                Categories = categories,
                Description = product.Description,
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ProductImages = product.ProductImages,
                Quantity = 1,
                Stock = product.Stock,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(AddProductToCartViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            Product product = await _context.Products.FindAsync(model.Id);
            if (product == null)
            {
                return NotFound();
            }
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
            TemporalSale temporalSale = new()
            {
                Product = product,
                Quantity = model.Quantity,
                Remarks = model.Remarks,
                User = user
            };
            _context.TemporalSales.Add(temporalSale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }

        public async Task<IActionResult> Add(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            Product product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            TemporalSale temporalSale = new()
            {
                Product = product,
                Quantity = 1,
                User = user
            };

            _context.TemporalSales.Add(temporalSale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Index(string search, int? categoryId, bool onSale = false)
        {
            IQueryable<Product> query = _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .Where(p => p.Stock > 0);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(p => p.ProductCategories.Any(pc => pc.Category.Id == categoryId.Value));
            }

            List<Product> products = await query
                .OrderBy(p => p.Name)
                .ToListAsync();

            if (onSale)
            {
                products = products.Where(p => DiscountHelper.HasDiscount(p.Id)).ToList();
            }

            HomeViewModel model = new()
            {
                Products = products,
                Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync(),
                Search = search,
                CategoryId = categoryId,
            };

            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user != null)
            {
                model.Quantity = await _context.TemporalSales
                    .Where(ts => ts.User.Id == user.Id)
                    .SumAsync(ts => ts.Quantity);
            }

            return View(model);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}