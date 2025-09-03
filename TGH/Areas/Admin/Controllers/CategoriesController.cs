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
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserContext _userContext;
        public CategoriesController(ApplicationDbContext context, UserContext userContext)
        {
            _context = context;
            _userContext = userContext;

            //if (User == null || !User.Identity.IsAuthenticated || userContext.User.Type != UserType.Admin)
            //    throw new UnauthorizedAccessException();
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated || _userContext.User.Type != UserType.Admin)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            return View(await _context.Category.Where(c => !c.IsDeleted).ToListAsync());
        }

        // GET: Admin/Categories/Details/5
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

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/Categories/Create
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated || _userContext.User.Type != UserType.Admin)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameEn,NameAr,Icon,IsActive")] Category category)
        {
            if (!User.Identity.IsAuthenticated || _userContext.User.Type != UserType.Admin)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/Categories/Edit/5
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

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameEn,NameAr,Icon,IsActive")] Category category)
        {
            if (!User.Identity.IsAuthenticated || _userContext.User.Type != UserType.Admin)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
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

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.Identity.IsAuthenticated || _userContext.User.Type != UserType.Admin)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var category = await _context.Category.FindAsync(id);

            category.IsActive = false;

            _context.Category.Update(category);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
