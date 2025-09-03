using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TGH.Data;
using TGH.Helpers;
using TGH.Models;

namespace TGH.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DonationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserContext _userContext;

        public DonationsController(ApplicationDbContext context, UserContext userContext)
        {
            _context = context;
            _userContext = userContext;

        }

        // GET: Admin/Donations
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated || (_userContext.User.Type != UserType.Admin && _userContext.User.Type != UserType.Approver))
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var applicationDbContext = _context.Donations.Include(i => i.Images).Include(d => d.Category).Include(d => d.City).Include(d => d.User);
            return View(await applicationDbContext.Where(d => !d.IsDeleted).ToListAsync());
        }

        // GET: Admin/Donations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!User.Identity.IsAuthenticated || (_userContext.User.Type != UserType.Admin && _userContext.User.Type != UserType.Approver))
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .Include(d => d.Category)
                .Include(d => d.City)
                .Include(d => d.User)
                .Include(d => d.Images)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // GET: Admin/Donations/Create
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated || (_userContext.User.Type != UserType.Admin && _userContext.User.Type != UserType.Approver))
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id");
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Admin/Donations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Title,Description,CategoryId,CityId,UserId,IsActive,IsDeleted,ShowName,Publish")] Donation donation)
        {
            if (!User.Identity.IsAuthenticated || (_userContext.User.Type != UserType.Admin && _userContext.User.Type != UserType.Approver))
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (ModelState.IsValid)
            {
                _context.Add(donation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", donation.CategoryId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id", donation.CityId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", donation.UserId);
            return View(donation);
        }

        // GET: Admin/Donations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated || (_userContext.User.Type != UserType.Admin && _userContext.User.Type != UserType.Approver))
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", donation.CategoryId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id", donation.CityId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", donation.UserId);
            return View(donation);
        }

        // POST: Admin/Donations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Title,Description,CategoryId,CityId,UserId,IsActive,IsDeleted,ShowName,Publish")] Donation donation)
        {
            if (!User.Identity.IsAuthenticated || (_userContext.User.Type != UserType.Admin && _userContext.User.Type != UserType.Approver))
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (id != donation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonationExists(donation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", donation.CategoryId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id", donation.CityId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", donation.UserId);
            return View(donation);
        }

        public async Task<IActionResult> Publish(int? id)
        {
            if (!User.Identity.IsAuthenticated || (_userContext.User.Type != UserType.Admin && _userContext.User.Type != UserType.Approver))
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (id == null)
            {
                return NotFound(); 
            }
            var donations = _context.Donations.Where(w => w.IsDeleted == false);
            var donation = await _context.Donations.FindAsync(id);
            donation.Publish = true;
            _context.Update(donation);
            await _context.SaveChangesAsync();
            if (donation == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", donation.CategoryId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id", donation.CityId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", donation.UserId);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Publish(int id, [Bind("Id,Date,Title,Description,CategoryId,CityId,UserId,IsActive,IsDeleted,ShowName,Publish")] Donation donation)
        {
            if (!User.Identity.IsAuthenticated || (_userContext.User.Type != UserType.Admin && _userContext.User.Type != UserType.Approver))
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
             
            if (id != donation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    donation.Publish = true;
                    _context.Update(donation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonationExists(donation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", donation.CategoryId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id", donation.CityId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", donation.UserId);
            return View(donation);
        }
        // GET: Admin/Donations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!User.Identity.IsAuthenticated || (_userContext.User.Type != UserType.Admin && _userContext.User.Type != UserType.Approver))
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .Include(d => d.Category)
                .Include(d => d.City)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // POST: Admin/Donations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.Identity.IsAuthenticated || (_userContext.User.Type != UserType.Admin && _userContext.User.Type != UserType.Approver))
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var donation = await _context.Donations.FindAsync(id);

            donation.IsDeleted = true;

            _context.Donations.Update(donation);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool DonationExists(int id)
        {
            return _context.Donations.Any(e => e.Id == id);
        }
    }
}
