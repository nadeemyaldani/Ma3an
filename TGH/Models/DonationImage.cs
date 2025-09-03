namespace TGH.Models
{
    public class DonationImage
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int DonationId { get; set; }
        public virtual Donation Donation { get; set; }
    }
}