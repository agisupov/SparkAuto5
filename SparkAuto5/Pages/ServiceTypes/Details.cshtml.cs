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

namespace SparkAuto5.Pages.ServiceTypes
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public DetailsModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public ServiceType ServiceType { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ServiceType = await _db.ServiceType.FirstOrDefaultAsync(m => m.Id == id);

            if (ServiceType == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
