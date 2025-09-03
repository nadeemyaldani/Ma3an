using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGH.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
}
