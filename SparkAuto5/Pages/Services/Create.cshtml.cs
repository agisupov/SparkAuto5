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
using SparkAuto5.Models.ViewModel;
using SparkAuto5.Utility;

namespace SparkAuto5.Pages.Services
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public CarServiceViewModel CarServiceViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? carId)
        {
            CarServiceViewModel = new CarServiceViewModel
            {
                Car = await _db.Car.Include(c => c.ApplicationUser).FirstOrDefaultAsync(c => c.Id == carId),
                ServiceHeader = new Models.ServiceHeader()
            };

            List<String> listServiceTypeInShoppingCart = _db.ServiceShoppingCart
                .Include(c => c.ServiceType)
                .Where(c => c.CarId == carId)
                .Select(c => c.ServiceType.Name)
                .ToList();

            IQueryable<ServiceType> listService = from s in _db.ServiceType
                                                  where !(listServiceTypeInShoppingCart.Contains(s.Name))
                                                  select s;
            CarServiceViewModel.ServiceTypesList = listService.ToList();
            CarServiceViewModel.ServiceShoppingCart = _db.ServiceShoppingCart.Include(c => c.ServiceType).Where(c => c.CarId == carId).ToList();
            CarServiceViewModel.ServiceHeader.TotalPrice = CarServiceViewModel.ServiceShoppingCart.Sum(item => item.ServiceType.Price);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                CarServiceViewModel.ServiceHeader.DateAdded = DateTime.Now;
                CarServiceViewModel.ServiceShoppingCart = _db.ServiceShoppingCart.Include(c => c.ServiceType).Where(c => c.CarId == CarServiceViewModel.Car.Id).ToList();
                CarServiceViewModel.ServiceHeader.TotalPrice = CarServiceViewModel.ServiceShoppingCart.Sum(s => s.ServiceType.Price);
                CarServiceViewModel.ServiceHeader.CarId = CarServiceViewModel.Car.Id;

                _db.ServiceHeader.Add(CarServiceViewModel.ServiceHeader);
                await _db.SaveChangesAsync();

                foreach (var detail in CarServiceViewModel.ServiceShoppingCart)
                {
                    ServiceDetails serviceDetails = new ServiceDetails
                    {
                        ServiceHeaderId = CarServiceViewModel.ServiceHeader.Id,
                        ServiceName = detail.ServiceType.Name,
                        ServicePrice = detail.ServiceType.Price,
                        ServiceTypeId = detail.ServiceTypeId
                    };

                    _db.ServiceDetails.Add(serviceDetails);
                }
                _db.ServiceShoppingCart.RemoveRange(CarServiceViewModel.ServiceShoppingCart);
                await _db.SaveChangesAsync();
                return RedirectToPage("../Cars/Index", new { userId = CarServiceViewModel.Car.UserId});
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCart()
        {
            ServiceShoppingCart objServiceCart = new ServiceShoppingCart()
            {
                CarId = CarServiceViewModel.Car.Id,
                ServiceTypeId = CarServiceViewModel.ServiceDetails.ServiceTypeId
            };
            _db.ServiceShoppingCart.Add(objServiceCart);
            await _db.SaveChangesAsync();
            return RedirectToPage("Create", new { carId = CarServiceViewModel.Car.Id });
        }

        public async Task<IActionResult> OnPostRemoveFromCart(int serviceTypeId)
        {
            ServiceShoppingCart objServiceCart = _db.ServiceShoppingCart
                .FirstOrDefault(u => u.CarId == CarServiceViewModel.Car.Id && u.ServiceTypeId == serviceTypeId);
            _db.ServiceShoppingCart.Remove(objServiceCart);
            await _db.SaveChangesAsync();
            return RedirectToPage("Create", new { carId = CarServiceViewModel.Car.Id });
        }
    }
}
