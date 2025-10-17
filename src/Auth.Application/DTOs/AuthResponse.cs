namespace Auth.Application.DTOs;
public record AuthResponse(string Token, DateTime ExpiresAtUtc);
