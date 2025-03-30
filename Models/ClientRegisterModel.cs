using System.ComponentModel.DataAnnotations;

public class ClientRegisterModel
{
    [Required]
    public string FName { get; set; } = string.Empty;

    [Required]
    public string LName { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; }
    [Required]
    public DateTime BirthDay { get; set; }

    [Required]
    public string Gender { get; set; } = string.Empty;
    [Required]
    [Range(50, 250, ErrorMessage = "Height must be between 50 and 250 cm.")]
    public int Hight { get; set; }

    [Required]
    [Range(20, 300, ErrorMessage = "Weight must be between 20 and 300 kg.")]
    public int Wight { get; set; }

     // استخدام enum بدلًا من string

    
}

// تعريف الـ Enum للجنس
//public enum Gender
//{
//    Male,
//    Female,

//}
