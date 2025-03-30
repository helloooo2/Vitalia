using System.ComponentModel.DataAnnotations;

namespace Vitalia.Models
{
    public class SellerRegisterModel
    {
        [Required]
        public string FName { get; set; } = string.Empty;
        [Required]
        public string LName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string BusinessLicenseNumber { get; set; } = string.Empty; // رقم الضريبة
        [Required]
        public DateTime LicenseExpiryDate { get; set; }
        [Required]
        public string StoreName { get; set; } = string.Empty;
    }
}
