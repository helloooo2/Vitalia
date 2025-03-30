using System.ComponentModel.DataAnnotations;

namespace Vitalia.Models
{
    public class DoctorRegisterModel
    {
        [Required]
        public string FName { get; set; } = string.Empty;

        [Required]
        public string LName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string LicenseNumber { get; set; } = string.Empty;
        [Required]
        public DateTime LicenseExpiryDate { get; set; }


    }
}
