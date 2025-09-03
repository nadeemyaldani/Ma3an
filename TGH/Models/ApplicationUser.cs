using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TGH.Models
{
    public class ApplicationUser : IdentityUser
    {
        public UserType Type { get; set; }
        public int? CityId { get; set; }
        public virtual City City { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public virtual List<Donation> Donations { get; set; }
        public virtual List<Conversation> Conversations { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
