using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TGH.Data;
using TGH.Models;

namespace TGH.Controllers
{
    public class DonationController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public DonationController(ILogger<HomeController> logger, ApplicationDbContext context) : base()
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Me(string key = null, int? donationId = null)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return LocalRedirect(Url.Action("Login", "User"));
            }

            var query = _context.Donations
                .Include(d => d.Category)
                .Include(d => d.City)
                .Include(d => d.User)
                .Include(d => d.Images).Where(d => !d.IsDeleted && d.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier) && d.IsActive);

            if (!key.IsEmpty())
            {
                query = query.Where(d => d.City != null && d.City.NameAR.Contains(key) || d.City.NameEN.Contains(key));
            }

            ViewData["Categories"] = _context.Category.Where(c => !c.IsDeleted && c.IsActive).ToList();

            ViewData["Key"] = key ?? string.Empty;

            if (donationId.HasValue)
            {
                ViewData["DonationId"] = donationId.Value;
            }

            return View(query.ToList());
        }
        public IActionResult Add()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return LocalRedirect(Url.Action("Login", "User"));
            }

            var categories = _context.Category.Where(c => !c.IsDeleted && c.IsActive).ToList();

            ViewData["Categories"] = categories.Select(item => new SelectListItem
            {
                Text = Arabic ? item.NameAr : item.NameEn,
                Value = item.Id.ToString()
            }).ToList();

            var cities = _context.Cities.Where(c => !c.IsDeleted && c.IsActive).ToList();

            ViewData["Cities"] = cities.Select(item => new SelectListItem
            {
                Text = Arabic ? item.NameAR : item.NameEN,
                Value = item.Id.ToString()
            }).ToList();

            return View();
        }
        [HttpPost]
        public IActionResult Add(DonationCreate donationCreate, IFormFileCollection formFiles)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return LocalRedirect(Url.Action("Login", "User"));
            }

            var categories = _context.Category.Where(c => !c.IsDeleted && c.IsActive).ToList();

            ViewData["Categories"] = categories.Select(item => new SelectListItem
            {
                Text = Arabic ? item.NameAr : item.NameEn,
                Value = item.Id.ToString(),
                Selected = item.Id == donationCreate.CategoryId
            }).ToList();

            var cities = _context.Cities.Where(c => !c.IsDeleted && c.IsActive).ToList();

            ViewData["Cities"] = cities.Select(item => new SelectListItem
            {
                Text = Arabic ? item.NameAR : item.NameEN,
                Value = item.Id.ToString(),
                Selected = item.Id == donationCreate.CityId
            }).ToList();

            if (ModelState.IsValid)
            {
                Donation donation = new Donation
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Description = donationCreate.Description,
                    CategoryId = donationCreate.CategoryId ?? 0,
                    CityId = donationCreate.CityId ?? 0,
                    Title = donationCreate.Title,
                    Date = DateTime.Now,
                    IsActive = true,
                    ShowName = donationCreate.ShowName
                };

                _context.Donations.Add(donation);

                _context.SaveChanges();

                using (MemoryStream ms = new MemoryStream())
                {
                    foreach (IFormFile file in formFiles)
                    {
                        file.CopyTo(ms);

                        byte[] content = ms.ToArray();

                        DonationImage donationImage = new DonationImage
                        {
                            DonationId = donation.Id,
                            Content = Convert.ToBase64String(content)
                        };

                        _context.DonationImages.Add(donationImage);
                    }
                }

                _context.SaveChanges();

                return LocalRedirect(Url.Action("Me", "Donation", new { donationId = donation.Id }));
            }

            return View();
        }

        public IActionResult Delete(int id)
        {
            var donation = _context.Donations.FirstOrDefault(d => d.Id == id);

            if (donation != null)
            {
                donation.IsDeleted = true;

                _context.Update(donation);

                _context.SaveChanges();
            }

            return LocalRedirect(Url.Action("Me", "Donation"));
        }

    }
}
