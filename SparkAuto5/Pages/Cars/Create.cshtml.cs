using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SparkAuto5.Data;
using SparkAuto5.Models;

namespace SparkAuto5.Pages.Cars
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Car Car { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult OnGet(string userId = null)
        {
            Car = new Car();
            if (userId is null)
            {
                var claimsIdenitity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdenitity.FindFirst(ClaimTypes.NameIdentifier);
                userId = claim.Value;
            }
            Car.UserId = userId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            StatusMessage = "Car has been added successfully!";
            _db.Car.Add(Car);
            await _db.SaveChangesAsync();

            return RedirectToPage("Index", new { userId = Car.UserId });
        }
    }
}
