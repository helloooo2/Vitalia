using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Vitalia.Models
{
    public class Seller 
    {
        public int Id { get; set; } // Primary Key
        public string UserId { get; set; } // Foreign Key to IdentityUser
        public IdentityUser User { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string FName { get; set; } = string.Empty;
        [Required]
        public string LName { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; } = string.Empty;
        [Required]
        public string StoreName { get; set; } = string.Empty; // اسم المتجر
        [Required]
        public string BusinessLicenseNumber { get; set; } = string.Empty; // رقم الضريبة
        [Required]
        public DateTime LicenseExpiryDate { get; set; }
        
        public string BankAccountInfo { get; set; }
      
        public string DocumentsPath { get; set; }
     
        public string ImagePath { get; set; }
    }
}
