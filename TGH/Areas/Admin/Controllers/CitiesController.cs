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
    public class CitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserContext _userContext;

        public CitiesController(ApplicationDbContext context, UserContext userContext)
        {
            _context = context;
            _userContext = userContext;

        }

        // GET: Admin/Cities
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated || _userContext.User.Type != UserType.Admin)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            return View(await _context.Cities.Where(c => !c.IsDeleted).ToListAsync());
        }

        // GET: Admin/Cities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!User.Identity.IsAuthenticated || _userContext.User.Type != UserType.Admin)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // GET: Admin/Cities/Create
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated || _userContext.User.Type != UserType.Admin)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            return View();
        }

        // POST: Admin/Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameAR,NameEN,IsActive")] City city)
        {
            if (!User.Identity.IsAuthenticated || _userContext.User.Type != UserType.Admin)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (ModelState.IsValid)
            {
                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(city);
        }

        // GET: Admin/Cities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated || _userContext.User.Type != UserType.Admin)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            return View(city);
        }

        // POST: Admin/Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameAR,NameEN,IsActive")] City city)
        {
            if (!User.Identity.IsAuthenticated || _userContext.User.Type != UserType.Admin)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (id != city.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.Id))
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
            return View(city);
        }

        // GET: Admin/Cities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!User.Identity.IsAuthenticated || _userContext.User.Type != UserType.Admin)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: Admin/Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.Identity.IsAuthenticated || _userContext.User.Type != UserType.Admin)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var city = await _context.Cities.FindAsync(id);

            city.IsDeleted = true;

            _context.Cities.Update(city);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(int id)
        {
            return _context.Cities.Any(e => e.Id == id);
        }
    }
}
