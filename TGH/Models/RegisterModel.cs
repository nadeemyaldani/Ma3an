using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;
using TGH.Helpers;
using TGH.Resources;
using System.Collections.Generic;
namespace TGH.Models
{
    public class RegisterModel
    {
        [CustomRegex("^(009627|9627|\\+9627|07|)(7|8|9)([0-9]{7})$", "VALIDATION_REGEX_PHONENUMBER")]
        //[CustomRegex("^[0-9]*$", "يرجى إدخال رقم هاتف صحيح")]
        [Required(ErrorMessageResourceName = "VALIDATION_PHONE_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceName = "VALIDATION_REGEX_PHONENUMBER", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_PHONE")]
        public string MobileNumber { get; set; }

        [EmailAddress(ErrorMessageResourceName = "VALIDATION_REGEX_EMAIL", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomRegex(UIHelper.EmailPattern, "VALIDATION_REGEX_EMAIL")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "VALIDATION_REGEX_EMAIL", ErrorMessageResourceType = typeof(SharedResource))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMAIL_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_EMAIL")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "VALIDATION_FULLNAME_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "VALIDATION_REGEX", ErrorMessageResourceType = typeof(SharedResource), MinimumLength = 6)]
        [CustomDisplay("FORM_FULLNAME")]
        public string FullName { get; set; }

        [StringLength(200, ErrorMessageResourceName = "VALIDATION_REGEX", ErrorMessageResourceType = typeof(SharedResource), MinimumLength = 6)]
        [CustomDisplay("FORM_DESCRIPTION")]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "VALIDATION_TYPE_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_TYPE")]
        public UserType Type { get; set; }

        [Required(ErrorMessageResourceName = "VALIDATION_CITY_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_CITY")]
        public int City { get; set; }

        [Required(ErrorMessageResourceName = "VALIDATION_PASSWORD_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "VALIDATION_REGEX", ErrorMessageResourceType = typeof(SharedResource), MinimumLength = 6)]
        [DataType(DataType.Password, ErrorMessageResourceName = "VALIDATION_PASSWORD_REGEX", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_PASSWORD")]
        public string Password { get; set; }

        //[Required(ErrorMessage = "يجب إدخال كلمة المرور")]
        //[StringLength(100, ErrorMessage = "يجب أن يكون {0} أكثر من {2} خانات و أقل من {1} خانة", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessageResourceName = "VALIDATION_CONFIRMPASSWORD_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_CONFIRMPASSWORD")]
        public string ConfirmPassword { get; set; }
    }
    public class ProfileUpdateModel
    {
        [CustomRegex("^(009627|9627|\\+9627|07|)(7|8|9)([0-9]{7})$", "VALIDATION_REGEX_PHONENUMBER")]
        //[Required(ErrorMessageResourceName = "VALIDATION_PHONE_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceName = "VALIDATION_REGEX_PHONENUMBER", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_PHONE")]
        public string MobileNumber { get; set; }

        [EmailAddress(ErrorMessageResourceName = "VALIDATION_REGEX_EMAIL", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomRegex(UIHelper.EmailPattern, "VALIDATION_REGEX_EMAIL")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "VALIDATION_REGEX_EMAIL", ErrorMessageResourceType = typeof(SharedResource))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMAIL_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_EMAIL")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "VALIDATION_FULLNAME_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_FULLNAME")]
        public string FullName { get; set; }

        [StringLength(200, ErrorMessageResourceName = "VALIDATION_REGEX", ErrorMessageResourceType = typeof(SharedResource), MinimumLength = 6)]
        [CustomDisplay("FORM_DESCRIPTION")]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "VALIDATION_TYPE_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_TYPE")]
        public UserType Type { get; set; }

        [Required(ErrorMessageResourceName = "VALIDATION_CITY_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_CITY")]
        public int City { get; set; }

        public string Image { get; set; }

        public ProfileUpdateModel()
        {

        }
        public ProfileUpdateModel(ApplicationUser user)
        {
            MobileNumber = user.PhoneNumber;
            Email = user.Email;
            FullName = user.FullName;
            Description = user.Description;
            Type = user.Type;
            City = user.CityId.Value;
            Image = user.Image;
        }
    }
    public class SecurityUpdateModel
    {
        [DataType(DataType.PhoneNumber, ErrorMessageResourceName = "VALIDATION_REGEX_PHONENUMBER", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_PHONE")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessageResourceName = "VALIDATION_OLDPASSWORD_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        //[StringLength(100, ErrorMessageResourceName = "VALIDATION_REGEX", ErrorMessageResourceType = typeof(SharedResource), MinimumLength = 6)]
        [DataType(DataType.Password, ErrorMessageResourceName = "VALIDATION_PASSWORD_REGEX", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_OLDPASSWORD")]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceName = "VALIDATION_NEWPASSWORD_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "VALIDATION_REGEX", ErrorMessageResourceType = typeof(SharedResource), MinimumLength = 6)]
        [DataType(DataType.Password, ErrorMessageResourceName = "VALIDATION_PASSWORD_REGEX", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_NEWPASSWORD")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessageResourceName = "VALIDATION_CONFIRMPASSWORD_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_NEWPASSWORDCONFIRM")]
        public string ConfirmPassword { get; set; }
        public SecurityUpdateModel()
        {

        }
        public SecurityUpdateModel(ApplicationUser user)
        {
            MobileNumber = user.PhoneNumber;
        }
    }
    public class LoginModel
    {
        //[CustomRegex("^07[789]-\\d{7}$", "VALIDATION_REGEX_PHONENUMBER")]
        //[CustomRegex("^[0-9]*$", "يرجى إدخال رقم هاتف صحيح")]

        //[CustomRequired("VALIDATION_PHONE_REQUIRED")]
        //[CustomDataType(DataType.PhoneNumber, "VALIDATION_REGEX_PHONENUMBER")]
        [RegularExpression("^(009627|9627|\\+9627|07|)(7|8|9)([0-9]{7})$", ErrorMessageResourceName = "VALIDATION_REGEX_PHONENUMBER", ErrorMessageResourceType = typeof(SharedResource))]
        [Required(ErrorMessageResourceName = "VALIDATION_PHONE_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceName = "VALIDATION_REGEX_PHONENUMBER", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_PHONE")]
        public string UserName { get; set; }

        //[CustomRequired("VALIDATION_PASSWORD_REQUIRED")]
        //[CustomString(6, 100, "VALIDATION_REGEX")]
        //[CustomDataType(DataType.Password, "VALIDATION_PASSWORD_REGEX")]
        [Required(ErrorMessageResourceName = "VALIDATION_PASSWORD_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "VALIDATION_REGEX", ErrorMessageResourceType = typeof(SharedResource), MinimumLength = 6)]
        [DataType(DataType.Password, ErrorMessageResourceName = "VALIDATION_PASSWORD_REGEX", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_PASSWORD")]
        public string Password { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class ForgotPasswordModel
    {
        [EmailAddress(ErrorMessageResourceName = "VALIDATION_REGEX_EMAIL", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomRegex(UIHelper.EmailPattern, "VALIDATION_REGEX_EMAIL")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "VALIDATION_REGEX_EMAIL", ErrorMessageResourceType = typeof(SharedResource))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMAIL_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_EMAIL")]
        public string Email { get; set; }
    }
    public class ResetPasswordModel
    {
        public string Code { get; set; }

        [EmailAddress(ErrorMessageResourceName = "VALIDATION_REGEX_EMAIL", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomRegex(UIHelper.EmailPattern, "VALIDATION_REGEX_EMAIL")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "VALIDATION_REGEX_EMAIL", ErrorMessageResourceType = typeof(SharedResource))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMAIL_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_EMAIL")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "VALIDATION_NEWPASSWORD_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "VALIDATION_REGEX", ErrorMessageResourceType = typeof(SharedResource), MinimumLength = 6)]
        [DataType(DataType.Password, ErrorMessageResourceName = "VALIDATION_PASSWORD_REGEX", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_NEWPASSWORD")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessageResourceName = "VALIDATION_CONFIRMPASSWORD_REQUIRED", ErrorMessageResourceType = typeof(SharedResource))]
        [CustomDisplay("FORM_NEWPASSWORDCONFIRM")]
        public string ConfirmPassword { get; set; }
    }
}
