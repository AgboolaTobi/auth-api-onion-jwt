using Auth.Application.Contracts;
using Auth.Application.DTOs;
using Auth.Domain.Repositories;
using Auth.Domain.Entities;

namespace Auth.Application.Services;


public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokens;

    public AuthService(IUserRepository users, IPasswordHasher hasher, ITokenService tokens)
        => (_users, _hasher, _tokens) = (users, hasher, tokens);

    public async Task<AuthResponse> RegisterAsync(RegisterRequest req, CancellationToken ct = default)
    {
        if (await _users.UsernameExistsAsync(req.Username, ct)) throw new InvalidOperationException("Username taken");
        if (await _users.EmailExistsAsync(req.Email, ct)) throw new InvalidOperationException("Email taken");

        var hash = _hasher.Hash(req.Password);
        var user = User.Create(req.Username.Trim(), req.Email.Trim().ToLowerInvariant(), hash);

        await _users.AddAsync(user, ct);
        await _users.SaveChangesAsync(ct);

        return _tokens.CreateToken(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest req, CancellationToken ct = default)
    {
        var user = await _users.GetByUsernameAsync(req.Username.Trim(), ct)
                   ?? throw new UnauthorizedAccessException("Invalid credentials");

        if (!_hasher.Verify(user.PasswordHash, req.Password))
            throw new UnauthorizedAccessException("Invalid credentials");

        return _tokens.CreateToken(user);
    }
}
