using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SparkAuto5.Data;
using SparkAuto5.Models.ViewModel;

namespace SparkAuto5.Pages.Cars
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public CarAndCustomerViewModel CarAndCustomerViewModel { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet(string userId = null)
        {
            if (userId is null){
                var claimsIdenitity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdenitity.FindFirst(ClaimTypes.NameIdentifier);
                userId = claim.Value;
            }

            CarAndCustomerViewModel = new CarAndCustomerViewModel()
            {
                Cars = await _db.Car.Where(c => c.UserId == userId).ToListAsync(),
                UserObj = await _db.ApplicationUser.FirstOrDefaultAsync(i => i.Id == userId)
            };

            return Page();
        }
    }
}
