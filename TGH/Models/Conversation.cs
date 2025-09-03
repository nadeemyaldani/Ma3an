using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TGH.Models
{
    public class Conversation
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int DonationId { get; set; }
        public virtual Donation Donation { get; set; }
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual ApplicationUser Customer { get; set; }
        public string DonatorId { get; set; }
        [ForeignKey("DonatorId")]
        public virtual ApplicationUser Donator { get; set; }
        public virtual List<Message> Messages { get; set; }
        public virtual List<Notification> Notifications { get; set; }
        public bool CustomerRead { get; set; }
        public bool DonatorRead { get; set; }
    }

    public class Message
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public string Text { get; set; }
        public DateTime When { get; set; }
        public string SenderId { get; set; }
        public bool SenderDeleted { get; set; } = false;
        public bool ReceiverDeleted { get; set; } = false;
    }
}
