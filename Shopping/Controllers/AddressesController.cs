using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Data.Entities;
using Shopping.Helpers;
using Shopping.Models;

namespace Shopping.Controllers
{
    [Authorize]
    public class AddressesController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ISelectListHelper _selectListHelper;

        public AddressesController(DataContext context, IUserHelper userHelper, ISelectListHelper selectListHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _selectListHelper = selectListHelper;
        }

        public async Task<IActionResult> Index()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            List<Address> addresses = await _context.Addresses
                .Include(a => a.City)
                .ThenInclude(c => c.State)
                .Where(a => a.User.Id == user.Id)
                .ToListAsync();

            return View(addresses);
        }

        public async Task<IActionResult> Create()
        {
            AddressViewModel model = new()
            {
                Countries = await _selectListHelper.GetComboCountriesAsync(),
                States = await _selectListHelper.GetComboStatesAsync(0),
                Cities = await _selectListHelper.GetComboCitiesAsync(0),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddressViewModel model)
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                model.Countries = await _selectListHelper.GetComboCountriesAsync();
                model.States = await _selectListHelper.GetComboStatesAsync(model.CountryId);
                model.Cities = await _selectListHelper.GetComboCitiesAsync(model.StateId);
                return View(model);
            }

            if (model.IsDefault)
            {
                List<Address> existing = await _context.Addresses
                    .Where(a => a.User.Id == user.Id)
                    .ToListAsync();
                foreach (Address address in existing)
                {
                    address.IsDefault = false;
                }
            }

            Address newAddress = new()
            {
                User = user,
                Recipient = model.Recipient,
                PhoneNumber = model.PhoneNumber,
                Street = model.Street,
                Neighborhood = model.Neighborhood,
                PostalCode = model.PostalCode,
                References = model.References,
                City = await _context.Cities.FindAsync(model.CityId),
                IsDefault = model.IsDefault,
            };

            _context.Addresses.Add(newAddress);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            Address address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == id && a.User.Id == user.Id);
            if (address == null)
            {
                return NotFound();
            }

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SetDefault(int id)
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            List<Address> addresses = await _context.Addresses
                .Where(a => a.User.Id == user.Id)
                .ToListAsync();

            foreach (Address address in addresses)
            {
                address.IsDefault = address.Id == id;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public JsonResult GetStates(int countryId)
        {
            Country country = _context.Countries
                .Include(c => c.States)
                .FirstOrDefault(c => c.Id == countryId);
            if (country == null)
            {
                return null;
            }

            return Json(country.States.OrderBy(d => d.Name));
        }

        public JsonResult GetCities(int stateId)
        {
            State state = _context.States
                .Include(s => s.Cities)
                .FirstOrDefault(s => s.Id == stateId);
            if (state == null)
            {
                return null;
            }

            return Json(state.Cities.OrderBy(c => c.Name));
        }
    }
}
