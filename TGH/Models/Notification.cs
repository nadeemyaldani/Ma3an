using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGH.Models
{
    public class Notification
    {
        public int ID { get; set; }
        public string Message { get; set; }
        public string MessageAR { get; set; }
        public bool IsRead { get; set; }
        public string Type { get; set; }
        public int RelatedItemID { get; set; }
        public string UserID { get; set; }
        public DateTime Date { get; set; }
    }
}
