using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SparkAuto5.Data;
using SparkAuto5.Models;

namespace SparkAuto5.Pages.Cars
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Car Car { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Car = await _db.Car.FirstOrDefaultAsync(m => m.Id == id);

            if (Car is null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var carFromDb = await _db.Car.FirstOrDefaultAsync(s => s.Id == Car.Id);
            carFromDb.VIN = Car.VIN;
            carFromDb.Make = Car.Make;
            carFromDb.Model = Car.Model;
            carFromDb.Style = Car.Style;
            carFromDb.Year = Car.Year;
            carFromDb.Kilometers = Car.Kilometers;
            carFromDb.Color = Car.Color;
            await _db.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
