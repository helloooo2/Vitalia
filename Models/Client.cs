using Microsoft.AspNetCore.Identity; // لازم تكون موجودة

namespace Vitalia.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; } // هنا لازم تكون IdentityUser مش User
        public string FName { get; set; }
        public string LName { get; set; }
        public string Address { get; set; }
        public double Hight { get; set; }
        public double Wight { get; set; }
        public DateTime BirthDay { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
    }
}