using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SparkAuto5.Data;
using SparkAuto5.Models;
using SparkAuto5.Models.ViewModel;
using SparkAuto5.Utility;

namespace SparkAuto5.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public UserListViewModel UsersListVM { get; set; }

        public async Task<IActionResult> OnGet(int productPage = 1, 
            string searchEmail = null, 
            string searchName = null, 
            string searchPhone = null)
        {
            UsersListVM = new UserListViewModel()
            {
                ApplicationUserList = await _db.ApplicationUser.ToListAsync()
            };

            StringBuilder param = new StringBuilder();
            param.Append("/Users?productPage=:");
            param.Append("&searchName=");
            if (searchName is not null)
            {
                param.Append(searchName);
            }
            param.Append("&searchEmail=");
            if (searchEmail is not null)
            {
                param.Append(searchEmail);
            }
            param.Append("&searchPhone=");
            if (searchPhone is not null)
            {
                param.Append(searchPhone);
            }

            if (searchEmail is not null)
                UsersListVM.ApplicationUserList = await _db.ApplicationUser
                    .Where(u => u.Email.ToLower().Contains(searchEmail.ToLower())).ToListAsync();
            else if (searchName is not null)
                UsersListVM.ApplicationUserList = await _db.ApplicationUser
                    .Where(u => u.Name.ToLower().Contains(searchName.ToLower())).ToListAsync();
            else if (searchPhone is not null)
                UsersListVM.ApplicationUserList = await _db.ApplicationUser
                    .Where(u => u.PhoneNumber.ToLower().Contains(searchPhone.ToLower())).ToListAsync();

            var count = UsersListVM.ApplicationUserList.Count();
            UsersListVM.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = SD.PaginationUsersPageSize,
                TotalItems = count,
                UrlParam = param.ToString()
            };

            UsersListVM.ApplicationUserList = UsersListVM.ApplicationUserList
                .OrderBy(p => p.Email)
                .Skip((productPage - 1) * SD.PaginationUsersPageSize)
                .Take(SD.PaginationUsersPageSize)
                .ToList();

            return Page();
        }
    }
}
