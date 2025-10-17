using System.ComponentModel.DataAnnotations;

namespace Auth.Application.DTOs;

public sealed class LoginRequest
{
    [Required]
    public string Username { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;
}
