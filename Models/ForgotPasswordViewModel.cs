using System.ComponentModel.DataAnnotations;

namespace Stomatologia.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string passwordResetCode { get; set; }

    }
}