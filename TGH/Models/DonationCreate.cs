using System.ComponentModel.DataAnnotations;
using TGH.Resources;

namespace TGH.Models
{
    public class DonationCreate
    {
        [Required(ErrorMessageResourceName = "VALIDATION_TITLE_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "VALIDATION_TITLE_REGEX", ErrorMessageResourceType = typeof(SharedResource), MinimumLength = 6)]
        [CustomDisplay("DONATION_FORM_TITLE")]
        public string Title { get; set; }
        [StringLength(100, ErrorMessageResourceName = "VALIATION_DESCRIPTION_REGEX", ErrorMessageResourceType = typeof(SharedResource), MinimumLength = 6)]
        [CustomDisplay("FORM_DESCRIPTION")]
        public string Description { get; set; }
        [Required(ErrorMessageResourceName = "VALIDATION_CATEGORY_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("DONATION_FORM_CATEGORY")]
        public int? CategoryId { get; set; }
        [Required(ErrorMessageResourceName = "VALIDATION_CITY_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_CITY")]
        public int? CityId { get; set; }
        public bool ShowName { get; set; }

    }
}
