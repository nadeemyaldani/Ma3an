using System;
using System.Collections.Generic;

namespace TGH.Models
{
    public class Donation
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<DonationImage> Images { get; set; } = new List<DonationImage>();
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool ShowName { get; set; } = false;
        public bool Publish { get; set; } = false;
    }
}
