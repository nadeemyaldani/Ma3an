using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TGH.Data;
using TGH.Models;

namespace TGH.Helpers
{
    public class UserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserContext(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        private async Task<ApplicationUser> GetUser()
        {
            var id = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _userManager.FindByIdAsync(id);
        }

        public ApplicationUser User => GetUser().Result;
    }
    public class NotificationContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        public NotificationContext(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        private List<Notification> GetNotification()
        {
            List<Notification> notifications = _context.Notifications.ToList();

            return notifications;
        }
        public List<Notification> Notifications => GetNotification();
    }
}
