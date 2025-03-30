using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Vitalia.Models
{
    public class Doctor 
    {
        public int Id { get; set; } // Primary Key
        public string UserId { get; set; } // Foreign Key to IdentityUser
        public IdentityUser User { get; set; }
        [Required]
        public string FName { get; set; } = string.Empty;

        [Required]
        public string LName { get; set; } = string.Empty;
        [Required]
        public string LicenseNumber { get; set; } = string.Empty;
        [Required]
        public DateTime LicenseExpiryDate { get; set; }
        [Required]
        public string LicenseIssuedBy { get; set; } = string.Empty;
        [Required]
        public string ScheduleAppointment { get; set; } = string.Empty;
        [Required]
        public string ShortBio { get; set; } = string.Empty;
        [Required]
        public string BankAccount { get; set; } = string.Empty;
        public string CertificationDocuments { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; }
    }
}
