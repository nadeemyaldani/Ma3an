using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TGH.Data;
using TGH.Helpers;
using TGH.Models;

namespace TGH.Controllers
{
    public class UserController : BaseController

    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly LanguageService _languageService;

        public UserController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ApplicationDbContext dbContext, LanguageService languageService) : base()
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _dbContext = dbContext;
            _languageService = languageService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return LocalRedirect(Url.Action("Index", "Home"));
            }

            var userTypes = Enum.GetValues(typeof(UserType)).Cast<UserType>().Where(o => o != 0);

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var item in userTypes)
            {
                items.Add(new SelectListItem
                {
                    Text = Arabic ? UIHelper.GetUserTypeName(item) : item.ToString(),
                    Value = item.ToString()
                });
            }

            ViewData["UserTypes"] = items;

            var cities = _dbContext.Cities.Where(c => !c.IsDeleted && c.IsActive).ToList();

            ViewData["Cities"] = cities.Select(item => new SelectListItem
            {
                Text = Arabic ? item.NameAR : item.NameEN,
                Value = item.Id.ToString()
            }).ToList();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model, IFormFile formFiles)
        {
            if (User.Identity.IsAuthenticated)
            {
                return LocalRedirect(Url.Action("Index", "Home"));
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.MobileNumber, Email = model.Email, PhoneNumber = model.MobileNumber, Description = model.Description, Type = model.Type, FullName = model.FullName, CityId = model.City };

                if (formFiles != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        formFiles.CopyTo(ms);

                        byte[] content = ms.ToArray();

                        var image = Convert.ToBase64String(content);

                        user.Image = image;
                    }
                }

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: true);
                    return LocalRedirect(Url.Action("Index", "Home"));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, _languageService.Getkey(error.Code));
                }
            }

            var userTypes = Enum.GetValues(typeof(UserType)).Cast<UserType>().Where(o => o != 0);

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var item in userTypes)
            {
                items.Add(new SelectListItem
                {
                    Text = Arabic ? UIHelper.GetUserTypeName(item) : item.ToString(),
                    Value = item.ToString()
                });
            }

            var cities = _dbContext.Cities.Where(c => !c.IsDeleted && c.IsActive).ToList();

            ViewData["Cities"] = cities.Select(item => new SelectListItem
            {
                Text = Arabic ? item.NameAR : item.NameEN,
                Value = item.Id.ToString()
            }).ToList();

            ViewData["UserTypes"] = items;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            LoginModel loginModel = new LoginModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(loginModel);
        }

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
                return LocalRedirect(Url.Action("Index", "Home"));
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
                        return LocalRedirect(Url.Action("Index", "Home"));
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

        [HttpPost]
        public  IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "User", new {ReturnUrl = returnUrl });

            var properties =  _signInManager.ConfigureExternalAuthenticationProperties(provider,redirectUrl);

            return new ChallengeResult(provider,properties);
        }
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteerror = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            LoginModel loginModel = new LoginModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            if (remoteerror != null)
            {
                ModelState.AddModelError(string.Empty, $"Error From External Provider: {remoteerror}.");
                return View("Login", loginModel);
            }
            //string googleApiKey = "AIzaSyCtKNUqn68boLc3IiiQeiFynlRs6GF4XaI";
            var info = await _signInManager.GetExternalLoginInfoAsync();
            //string googleApiKey = "AIzaSyCtKNUqn68boLc3IiiQeiFynlRs6GF4XaI";
            //string nameIdentifier = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            //string jsonUrl = $"https://www.googleapis.com/plus/v1/people/{nameIdentifier}?fields=image&key={googleApiKey}";
            //using (HttpClient httpClient = new HttpClient())
            //{
            //    string s = await httpClient.GetStringAsync(jsonUrl);
            //    dynamic deserializeObject = JsonConvert.DeserializeObject(s);
            //    string thumbnailUrl = (string)deserializeObject.image.url;
            //    byte[] thumbnail = await httpClient.GetByteArrayAsync(thumbnailUrl);
            //}
            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "Error Loading Login Info.");
                return View("Login", loginModel);
            }
            var signinresult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signinresult.Succeeded)
            {
                //var userProfile = await _userManager.GetUserAsync(User);
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var user = await _userManager.FindByEmailAsync(email);
                byte[] thumbnail = null;
                byte[] content = null;
                if (user != null)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        string thumbnailUrl = string.Empty;
                        if (info.ProviderDisplayName.ToLower() == "google")
                        {
                            thumbnailUrl = info.Principal.FindFirstValue("picture");
                        }
                        else if(info.ProviderDisplayName.ToLower() == "facebook")
                        {
                            var identifier = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                            thumbnailUrl = $"https://graph.facebook.com/{identifier}/picture?type=large";
                        }
                        if(thumbnailUrl != string.Empty)
                            thumbnail = await httpClient.GetByteArrayAsync(thumbnailUrl);
                    }
                    using (MemoryStream ms = new MemoryStream())
                    {
                        var image = string.Empty;
                        if (thumbnail != null)
                            content = thumbnail.ToArray();
                        if(content != null)
                            image = Convert.ToBase64String(content);

                        
                        user.Image = image;
                        
                    }

                    _dbContext.Update(user);
                }
                return LocalRedirect(returnUrl);
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    byte[] thumbnail;
                    byte[] content;
                    if (user == null)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            string thumbnailUrl = info.Principal.FindFirstValue("picture");
                            thumbnail = await httpClient.GetByteArrayAsync(thumbnailUrl);
                        }
                        using (MemoryStream ms = new MemoryStream())
                        {
                            content = thumbnail.ToArray();

                            var image = Convert.ToBase64String(content);
                            user = new ApplicationUser
                            {
                                UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                                Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                                FullName = info.Principal.FindFirstValue(ClaimTypes.Name),
                                Type = UserType.Public,
                                CityId = 1,
                                Image = image,
                                //Image = info.ExternalIdentity.Claims.First(a => a.Type == "urn:google:picture"),
                            };
                        }
                    

                        await _userManager.CreateAsync(user);
                    }

                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }
                ViewBag.ErrorTitle = $"Email claims not recieved from {info.LoginProvider}";
                ViewBag.ErrorMessage = $"Please Contact Support";

                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            return LocalRedirect(Url.Action("Index", "Home"));
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string returnUrl = null)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return LocalRedirect(Url.Action("Login", "User"));
            }

            var userProfile = await _userManager.GetUserAsync(User);

            var userTypes = Enum.GetValues(typeof(UserType)).Cast<UserType>().Where(o => o != 0);

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var item in userTypes)
            {
                items.Add(new SelectListItem
                {
                    Text = Arabic ? UIHelper.GetUserTypeName(item) : item.ToString(),
                    Value = item.ToString(),
                    Selected = item == userProfile.Type ? true : false
                });
            }

            var cities = _dbContext.Cities.Where(c => !c.IsDeleted && c.IsActive).ToList();

            ViewData["Cities"] = cities.Select(item => new SelectListItem
            {
                Text = Arabic ? item.NameAR : item.NameEN,
                Value = item.Id.ToString(),
                Selected = item.Id == userProfile.CityId ? true : false
            }).ToList();

            ViewData["UserTypes"] = items;

            ProfileUpdateModel profileUpdate = new ProfileUpdateModel(userProfile);

            return View(profileUpdate);
        }
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileUpdateModel profileUpdate, IFormFile formFiles)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return LocalRedirect(Url.Action("Login", "User"));
            }

            var userProfile = await _userManager.GetUserAsync(User);

            userProfile.FullName = profileUpdate.FullName;
            userProfile.Email = profileUpdate.Email;
            userProfile.Description = profileUpdate.Description;
            userProfile.CityId = profileUpdate.City;
            userProfile.PhoneNumber = profileUpdate.MobileNumber;

            if (userProfile.Type != UserType.Admin && userProfile.Type != UserType.Approver)
            {
                userProfile.Type = profileUpdate.Type;
            }

            if (formFiles != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    formFiles.CopyTo(ms);

                    byte[] content = ms.ToArray();

                    var image = Convert.ToBase64String(content);

                    userProfile.Image = image;
                }
            }

            profileUpdate = new ProfileUpdateModel(userProfile);

            var userTypes = Enum.GetValues(typeof(UserType)).Cast<UserType>().Where(o => o != 0);

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var item in userTypes)
            {
                items.Add(new SelectListItem
                {
                    Text = Arabic ? UIHelper.GetUserTypeName(item) : item.ToString(),
                    Value = item.ToString(),
                    Selected = item == userProfile.Type ? true : false
                });
            }

            var cities = _dbContext.Cities.Where(c => !c.IsDeleted && c.IsActive).ToList();

            ViewData["Cities"] = cities.Select(item => new SelectListItem
            {
                Text = Arabic ? item.NameAR : item.NameEN,
                Value = item.Id.ToString(),
                Selected = item.Id == userProfile.CityId ? true : false
            }).ToList();

            ViewData["UserTypes"] = items;

            if (!ModelState.IsValid)
            {
                return View(profileUpdate);
            }

            var result = await _userManager.UpdateAsync(userProfile);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, _languageService.Getkey("ERROR_UPDATEFAILED"));

                return View(profileUpdate);
            }

            return View(profileUpdate);
        }
        [HttpGet]
        public async Task<IActionResult> Security(string returnUrl = null)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return LocalRedirect(Url.Action("Login", "User"));
            }

            var userProfile = await _userManager.GetUserAsync(User);

            SecurityUpdateModel securityUpdate = new SecurityUpdateModel(userProfile);

            return View(securityUpdate);
        }
        [HttpPost]
        public async Task<IActionResult> Security(SecurityUpdateModel securityUpdate)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return LocalRedirect(Url.Action("Login", "User"));
            }

            var userProfile = await _userManager.GetUserAsync(User);

            securityUpdate = new SecurityUpdateModel(userProfile);

            if (!ModelState.IsValid)
            {
                return View(securityUpdate);
            }

            var result = await _userManager.ChangePasswordAsync(userProfile, securityUpdate.OldPassword, securityUpdate.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, ResourceHelper.GetKey(error.Code));
                }

                return View(securityUpdate);
            }

            return View(securityUpdate);
        }
        [HttpGet]
        public async Task<bool> DeleteImage()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return false;
            }

            var userProfile = await _userManager.GetUserAsync(User);

            userProfile.Image = null;

            var result = await _userManager.UpdateAsync(userProfile);

            if (result != null)
            {
                return true;
            }

            return false;
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = Url.Action("ResetPassword", "User", new { code }, Request.Scheme);

                Email email = new Email
                {
                    EmailAddress = forgotPasswordModel.Email,
                    Subject = _languageService.Getkey("EMAIL_RESET_TITLE"),
                    Body = string.Format(_languageService.Getkey("EMAIL_RESET_BODY"), HtmlEncoder.Default.Encode(callbackUrl)),
                    IsHtml = true
                };

                EmailService.SendEmail(email);

                return LocalRedirect(Url.Action("ForgotPasswordConfirmation", "User"));
            }

            return View(forgotPasswordModel);
        }
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string code)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);

            if (user == null)
            {
                // Don't reveal that the user does not exist
                return LocalRedirect(Url.Action("ResetPasswordConfirmation", "User"));
            }

            var result = await _userManager.ResetPasswordAsync(user, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordModel.Code)), resetPasswordModel.NewPassword);

            if (result.Succeeded)
            {
                return LocalRedirect(Url.Action("ResetPasswordConfirmation", "User"));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, _languageService.Getkey(error.Code));
            }

            return View(resetPasswordModel);
        }
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}
