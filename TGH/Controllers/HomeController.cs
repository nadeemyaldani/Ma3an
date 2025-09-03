using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TGH.Data;
using TGH.Helpers;
using TGH.Models;

namespace TGH.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context) : base()
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(string key = null)
        {
            var test = CultureInfo.CurrentUICulture.Name;
            var cities = _context.Cities.ToList();
            //HttpContext.Response.Cookies.Append("language", "ar");
            var query = _context.Donations
               .Include(d => d.Category)
               .Include(d => d.City)
               .Include(d => d.User)
               .Include(d => d.Images).Where(d => !d.IsDeleted && d.IsActive && d.Publish);

            if (!key.IsEmpty())
            {
                query = query.Where(d => d.City != null && d.City.NameAR.Contains(key) || d.City.NameEN.Contains(key));
            }

            ViewData["Categories"] = _context.Category.Where(c => !c.IsDeleted && c.IsActive).ToList();

            ViewData["Key"] = key ?? string.Empty;
            ViewData["Cities"] = cities.ToList();
            return View(query.ToList());
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult DonationConditions()
        {
            return View();
        }
        public IActionResult Chat(int? conversationId = null)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return LocalRedirect(Url.Action("Login", "User"));
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var conversations = _context.Conversations
                .Include(c => c.Customer)
                .Include(c => c.Donation).ThenInclude(i => i.Images)
                .Include(c => c.Donator)
                .Include(c => c.Notifications)
                .Where(c => !c.Donation.IsDeleted && (c.DonatorId == userId || c.CustomerId == userId))
                .OrderByDescending(c => c.Date)
                .ToList();

            if (conversationId.HasValue)
            {
                ViewData["ConversationId"] = conversationId;
            }

            return View(conversations);
        }

        public IActionResult CreateChat(int donationId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return LocalRedirect(Url.Action("Login", "User"));
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var donation = _context.Donations.FirstOrDefault(d => d.Id == donationId);

            var existingConversation = _context.Conversations.FirstOrDefault(d => d.DonationId == donationId && (d.DonatorId == userId || d.CustomerId == userId));

            if (existingConversation != null)
            {
                return LocalRedirect(Url.Action("Chat", "Home", new { conversationId = existingConversation.Id }));
            }

            var conversation = new Conversation
            {
                CustomerId = userId,
                DonatorId = donation.UserId,
                DonationId = donationId,
                Date = DateTime.Now
            };

            _context.Conversations.Add(conversation);

            _context.SaveChanges();

            return LocalRedirect(Url.Action("Chat", "Home", new { conversationId = conversation.Id }));
        }

        public IActionResult Charities(int? cityId = null)
        {
            var charities = _context.Users.Where(u =>  (!cityId.HasValue || u.CityId == cityId)).Include(o => o.City).ToList();

            var cities = _context.Cities.Where(c => !c.IsDeleted && c.IsActive).ToList();

            ViewData["Cities"] = cities;

            ViewData["CityId"] = cityId;

            return View(charities);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public int SendMessage(int conversationId, string message)
        {
            try
            {
                var convoMessage = new Message
                {
                    ConversationId = conversationId,
                    Text = message,
                    When = DateTime.Now,
                    SenderId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                };
                var conversation = _context.Conversations.Where(w => w.Id == conversationId).FirstOrDefault();
                Notification Notification = new()
                {
                    IsRead = false,
                    Message = message.Length > 20 ? message.Substring(0,20) + "...": message,
                    MessageAR = message.Length > 20 ? message.Substring(0, 20) + "..." : message,
                    Date = DateTime.Now,
                    RelatedItemID = conversationId,
                    UserID = User.FindFirstValue(ClaimTypes.NameIdentifier) == conversation.DonatorId ? conversation?.CustomerId : conversation.DonatorId,

                };
                _context.Messages.Add(convoMessage);
                _context.Notifications.Add(Notification);

                _context.SaveChanges();

                return convoMessage.Id;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public string GetMessages(int conversationId)
        {
            var conversation = _context.Conversations.Include(c => c.Messages).Include(c => c.Customer).Include(c => c.Donator).Include(d => d.Donation).ThenInclude(i => i.Images).FirstOrDefault(c => c.Id == conversationId);

            if (conversation == null)
            {
                return null;
            }

            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == conversation.DonatorId)
            {
                conversation.DonatorRead = true;
            }
            else
            {
                conversation.CustomerRead = true;
            }

            _context.Conversations.Update(conversation);
            ClickNotificationPage(conversationId);

            _context.SaveChanges();

            var html = this.RenderViewAsync("Messages", conversation, false).Result;
            return html;
        }
        private string ClickNotificationPage(int conversationId)
        {
            var conversation = _context.Conversations.Include(c => c.Messages).Include(c => c.Customer).Include(c => c.Donator).Include(d => d.Donation).ThenInclude(i => i.Images).FirstOrDefault(c => c.Id == conversationId);
            var notifications = _context.Notifications.Where(w => w.RelatedItemID == conversationId).ToList();

            if (conversation == null)
            {
                return null;
            }

            foreach (var item in notifications)
            {
                item.IsRead = true;
                _context.Notifications.Update(item);
            }

            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == conversation.DonatorId)
            {
                conversation.DonatorRead = true;
            }
            else
            {
                conversation.CustomerRead = true;
            }

            _context.Conversations.Update(conversation);

            _context.SaveChanges();

            var html = this.RenderViewAsync("Messages", conversation, false).Result;
            return html;
        }
        public int ClickNotification(int conversationId = 0, int notificationId = 0)
        {
            var conversation = _context.Conversations.Include(c => c.Messages).Include(c => c.Customer).Include(c => c.Donator).Include(d => d.Donation).ThenInclude(i => i.Images).FirstOrDefault(c => c.Id == conversationId);
            var notifications = _context.Notifications.Where(w => w.RelatedItemID == conversationId).ToList();
            var notification = _context.Notifications.Where(w => w.ID == notificationId).FirstOrDefault();

            if (conversation == null)
            {
                return 0;
            }

            if (notificationId == 0)
            {
                foreach (var item in notifications)
                {
                    item.IsRead = true;
                    _context.Notifications.Update(item);
                }

            }
            else
            {
                if (notification == null)
                {
                    return 0;
                }
                notification.IsRead = true;
                _context.Notifications.Update(notification);
            }
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == conversation.DonatorId)
            {
                conversation.DonatorRead = true;
            }
            else
            {
                conversation.CustomerRead = true;
            }

            _context.Conversations.Update(conversation);

            _context.SaveChanges();

            //return LocalRedirect(Url.Action("CreateChat", "Home", new { donationId = conversation.DonationId }));
            var html = this.RenderViewAsync("Messages", conversation, false).Result;
            return conversation.Id;

        }
        public bool DeleteMessage(int id)
        {
            var message = _context.Messages.FirstOrDefault(m => m.Id == id);

            if (message.SenderId == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                message.SenderDeleted = true;
            }
            else
            {
                message.ReceiverDeleted = true;
            }

            _context.SaveChanges();

            return true;
        }
    }
}
