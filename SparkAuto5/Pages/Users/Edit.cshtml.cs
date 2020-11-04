using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SparkAuto5.Data;
using SparkAuto5.Models;
using SparkAuto5.Utility;

namespace SparkAuto5.Pages.Users
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class EditModel : PageModel
    {
        public readonly ApplicationDbContext _db;

        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id is null)
            {
                return NotFound();
            }

            ApplicationUser = await _db.ApplicationUser.FirstOrDefaultAsync(m => m.Id == id);

            if (ApplicationUser is null)
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

            var applicationUserFromDb = await _db.ApplicationUser.FirstOrDefaultAsync(s => s.Id == ApplicationUser.Id);
            applicationUserFromDb.Name = ApplicationUser.Name;
            applicationUserFromDb.PhoneNumber = ApplicationUser.PhoneNumber;
            applicationUserFromDb.Address = ApplicationUser.Address;
            applicationUserFromDb.City = ApplicationUser.City;
            applicationUserFromDb.PostalCode = ApplicationUser.PostalCode;
            await _db.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
