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
    public class DeleteModel : PageModel
    {
        public readonly ApplicationDbContext _db;

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser = await _db.ApplicationUser.FirstOrDefaultAsync(m => m.Id == id);

            if (ApplicationUser == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id is null)
            {
                return NotFound();
            }

            ApplicationUser = await _db.ApplicationUser.FindAsync(id);

            if (ApplicationUser is not null)
            {
                _db.ApplicationUser.Remove(ApplicationUser);
                await _db.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }
    }
}
