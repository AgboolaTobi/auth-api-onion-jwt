using System.ComponentModel.DataAnnotations;

namespace Auth.Application.DTOs;

public sealed class RegisterRequest
{
    [Required, MinLength(3)]
    public string Username { get; set; } = default!;

    
    [Required, EmailAddress, MaxLength(254)]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        ErrorMessage = "Email must include a valid domain (e.g., sample.com).")]
    public string Email { get; set; } = default!;

    [Required, MinLength(8)]
    public string Password { get; set; } = default!;
}
