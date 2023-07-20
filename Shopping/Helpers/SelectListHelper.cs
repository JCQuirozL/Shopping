using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using System.Collections.Generic;

namespace Shopping.Helpers
{
    public class SelectListHelper : ISelectListHelper
    {
        private readonly DataContext _context;

        public SelectListHelper(DataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SelectListItem>> GetComboCategoriesAsync()
        {
            List<SelectListItem> list = await _context.Categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = $"{x.Id}"
            }).OrderBy(x => x.Text)
              .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una categoría...]",
                Value = "0"
            });

            return list;

        }

        public async Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int stateId)
        {
            List<SelectListItem> list = await _context.Cities
              .Where(x => x.State.Id == stateId)
              .Select(x => new SelectListItem
              {
                  Text = x.Name,
                  Value = $"{x.Id}"
              })
              .OrderBy(x => x.Text)
              .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una ciudad...]",
                Value = "0"
            });

            return list;

        }

        public async Task<IEnumerable<SelectListItem>> GetComboCountriesAsync()
        {
            List<SelectListItem> list = await _context.Countries.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = $"{x.Id}"
            })
                            .OrderBy(x => x.Text)
                            .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un país...]",
                Value = "0"
            });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboStatesAsync(int countryId)
        {
            List<SelectListItem> list = await _context.States.Where(s => s.Country.Id == countryId)
                .Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = $"{s.Id}"
                }).OrderBy(s => s.Text)
            .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione departamento/estado]",
                Value = "0",
            });

            return list;
        }
    }
}
