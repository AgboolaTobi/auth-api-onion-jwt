using Auth.Application.DTOs;
using Auth.Domain.Entities;

namespace Auth.Application.Contracts;

public interface ITokenService
{
    AuthResponse CreateToken(User user);
}
