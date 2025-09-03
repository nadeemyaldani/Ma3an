using System;
using System.Collections.Generic;

namespace TGH.Models
{
    public class City
    {
        public int Id { get; set; }
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public virtual List<ApplicationUser> ApplicationUsers { get; set; }
    }
}
