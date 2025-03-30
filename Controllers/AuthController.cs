using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Vitalia.Data;
using Vitalia.Models;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register/client")]
    public async Task<IActionResult> RegisterClient([FromBody] ClientRegisterModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { message = "Invalid data provided." });

        var user = new IdentityUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return BadRequest(new { message = "Registration failed.", errors = result.Errors });

        var client = new Client
        {
            UserId = user.Id,
            FName = model.FName,
            LName = model.LName,
            Hight = model.Hight,
            Wight = model.Wight,
            BirthDay = model.BirthDay,
            Gender = model.Gender,
            Phone = model.Phone
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        if (!await _roleManager.RoleExistsAsync("Client"))
            await _roleManager.CreateAsync(new IdentityRole("Client"));

        await _userManager.AddToRoleAsync(user, "Client");

        return Ok(new { message = "Client registered successfully!" });
    }

    [HttpPost("register/doctor")]
    public async Task<IActionResult> RegisterDoctor([FromBody] DoctorRegisterModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { message = "Invalid data provided." });

        var user = new IdentityUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return BadRequest(new { message = "Registration failed.", errors = result.Errors });

        var doctor = new Doctor
        {
            UserId = user.Id,
            FName = model.FName,
            LName = model.LName,
            Phone = model.Phone,
            LicenseNumber = model.LicenseNumber,
            LicenseExpiryDate = model.LicenseExpiryDate
        };

        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();

        if (!await _roleManager.RoleExistsAsync("Doctor"))
            await _roleManager.CreateAsync(new IdentityRole("Doctor"));

        await _userManager.AddToRoleAsync(user, "Doctor");

        return Ok(new { message = "Doctor registered successfully!" });
    }
    [HttpPost("register/seller")]
    public async Task<IActionResult> RegisterSeller([FromBody] SellerRegisterModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { message = "Invalid data provided." });

        // Check if the email is already registered
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
            return BadRequest(new { message = "Email is already registered." });

        // Create a new IdentityUser
        var user = new IdentityUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return BadRequest(new { message = "Registration failed.", errors = result.Errors });

        // Add Seller-specific details to the database
        var seller = new Seller
        {
            UserId = user.Id,
            FName = model.FName,
            LName = model.LName,
            Phone = model.Phone,
            BusinessLicenseNumber = model.BusinessLicenseNumber,
            LicenseExpiryDate = model.LicenseExpiryDate,
            StoreName = model.StoreName
        };

        _context.Sellers.Add(seller);
        await _context.SaveChangesAsync();

        // Ensure the "Seller" role exists
        if (!await _roleManager.RoleExistsAsync("Seller"))
            await _roleManager.CreateAsync(new IdentityRole("Seller"));

        // Assign the "Seller" role to the user
        await _userManager.AddToRoleAsync(user, "Seller");

        return Ok(new { message = "Seller registered successfully!" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { message = "Invalid data provided." });

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !(await _userManager.CheckPasswordAsync(user, model.Password)))
            return Unauthorized(new { message = "Invalid email or password" });

        var jwtKey = _configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 16)
            return StatusCode(500, new { message = "Server configuration error: Invalid JWT key." });

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtKey);
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email)
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { Token = tokenString });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
    {
        if (string.IsNullOrEmpty(model.Email))
            return BadRequest(new { message = "Email is required." });

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return BadRequest(new { message = "User not found." });

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetLink = $"https://yourfrontend.com/reset-password?email={model.Email}&token={WebUtility.UrlEncode(token)}";

        // هنا كنا بنرسل البريد الإلكتروني، لكن دلوقتي هنطبع الرسالة في الـ Console
        Console.WriteLine($"Password Reset Link: {resetLink}");

        return Ok(new { Message = "Password reset link generated.", ResetLink = resetLink });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Token) || string.IsNullOrEmpty(model.NewPassword))
            return BadRequest(new { message = "Invalid request." });

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return BadRequest(new { message = "User not found." });

        var resetResult = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
        if (!resetResult.Succeeded)
            return BadRequest(new { message = "Password reset failed.", errors = resetResult.Errors });

        return Ok(new { Message = "Password has been reset successfully." });
    }
}