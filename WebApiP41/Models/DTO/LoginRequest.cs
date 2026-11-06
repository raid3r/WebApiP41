using System.ComponentModel.DataAnnotations;

namespace WebApiP41.Models.DTO;

/// <summary>
/// Login request with email and password
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Email (as login)
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string Email { get; set; }

    /// <summary>
    /// User password
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; }
}
