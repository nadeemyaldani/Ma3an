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
using Microsoft.AspNetCore.Identity;

namespace TGH.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserContext _userContext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly LanguageService _languageService;
        //public UsersController(ApplicationDbContext context, UserContext userContext)
        //{
        //    _context = context;
        //    _userContext = userContext;
        //}
        public UsersController(ApplicationDbContext context, UserContext userContext, UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager, ApplicationDbContext dbContext, LanguageService languageService) : base()
        {
            _context = context;
            _userContext = userContext;
            _signInManager = signInManager;
            _userManager = userManager;
            _dbContext = dbContext;
            _languageService = languageService;
        }


        // GET: Admin/ApplicationUsers
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated || _userContext.User.Type != UserType.Admin)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var applicationDbContext = _context.ApplicationUsers.Include(u => u.City);
            return View(await applicationDbContext.ToListAsync());
        }

        #region
        [HttpGet]
        public async Task<IActionResult> AdminLogin(string returnUrl = null)
        {
            LoginModel loginModel = new LoginModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(LoginModel loginModel, string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(Url.Action("Index", "Categories"),new { area = "Admin" });
            }
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginModel.UserName);

                if (!user.IsActive)
                {
                    ModelState.AddModelError(string.Empty, _languageService.Getkey("USER_INACTIVE"));
                    return View();
                }

                var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, true, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (returnUrl != null && returnUrl != "")
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return LocalRedirect(Url.Action("Index", "Categories"));
                    }
                }
                if (result.IsLockedOut)
                {
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, _languageService.Getkey("ERROR_LOGGINGIN"));
                    return View();
                }
            }

            return View();
        }
        #endregion  


        // GET: Admin/ApplicationUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (!User.Identity.IsAuthenticated || _userContext.User.Type != UserType.Admin)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers
                .Include(u => u.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        //// GET: Admin/ApplicationUsers/Create
        //public IActionResult Create()
        //{
        //    if (!User.Identity.IsAuthenticated || _userContext.User.Type != UserType.Admin)
        //    {
        //        return RedirectToAction("Index", "Home", new { area = "" });
        //    }

        //    ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id");
        //    return View();
        //}

        //// POST: Admin/ApplicationUsers/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,PhoneNumber,Email,ApplicationUserName,Type,CityId,FullName,Image,Description,IsActive")] ApplicationUser user)
        //{
        //    if (!User.Identity.IsAuthenticated || _userContext.User.Type != UserType.Admin)
        //    {
        //        return RedirectToAction("Index", "Home", new { area = "" });
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(user);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id", user.CityId);
        //    return View(user);
        //}

        // GET: Admin/ApplicationUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (!User.Identity.IsAuthenticated || _userContext.User.Type != UserType.Admin)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (id == null)
            {
                return NotFound();
            }

            var userTypes = Enum.GetValues(typeof(UserType)).Cast<UserType>().ToList();

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var item in userTypes)
            {
                items.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    Value = item.ToString()
                });
            }

            ViewData["UserTypes"] = items;

            var user = await _context.ApplicationUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var cities = _context.Cities.Where(c => !c.IsDeleted && c.IsActive).ToList();

            ViewData["CityId"] = cities.Select(item => new SelectListItem
            {
                Text = item.NameEN,
                Value = item.Id.ToString()
            }).ToList();

            return View(user);
        }

        // POST: Admin/ApplicationUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,PhoneNumber,Email,UserName,Type,CityId,FullName,Description,IsActive")] ApplicationUser user)
        {
            if (!User.Identity.IsAuthenticated || _userContext.User.Type != UserType.Admin)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldUser = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == id);

                    oldUser.PhoneNumber = user.PhoneNumber;
                    oldUser.Email = user.Email;
                    oldUser.UserName = user.PhoneNumber;
                    oldUser.Type = user.Type;
                    oldUser.CityId = user.CityId;
                    oldUser.FullName = user.FullName;
                    oldUser.Description = user.Description;
                    oldUser.IsActive = user.IsActive;

                    _context.ApplicationUsers.Update(oldUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(user.Id))
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

            var userTypes = Enum.GetValues(typeof(UserType)).Cast<UserType>().ToList();

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var item in userTypes)
            {
                items.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    Value = item.ToString()
                });
            }

            ViewData["UserTypes"] = items;

            var cities = _context.Cities.Where(c => !c.IsDeleted && c.IsActive).ToList();

            ViewData["CityId"] = cities.Select(item => new SelectListItem
            {
                Text = item.NameEN,
                Value = item.Id.ToString()
            }).ToList();

            return View(user);
        }

        // GET: Admin/ApplicationUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (!User.Identity.IsAuthenticated || _userContext.User.Type != UserType.Admin)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers
                .Include(u => u.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (!User.Identity.IsAuthenticated || _userContext.User.Type != UserType.Admin)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var user = await _context.ApplicationUsers.FindAsync(id);
            _context.ApplicationUsers.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUsers.Any(e => e.Id == id);
        }
    }
}
