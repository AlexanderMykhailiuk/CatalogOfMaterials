using System.ComponentModel.DataAnnotations;

namespace WEBLayer.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Old password is required")]
        [Display(Name = "Old password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [Display(Name = "New password (you will use for logging in)")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat password")]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}