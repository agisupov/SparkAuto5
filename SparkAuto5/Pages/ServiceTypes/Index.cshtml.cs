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
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public IList<ServiceType> ServiceType { get; set; }
        public async Task<IActionResult> OnGet()
        {
            ServiceType = await _db.ServiceType.ToListAsync();
            return Page();
        }
    }
}
