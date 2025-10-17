using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Auth.Domain.Entities;

public class User
{
    private User() { }
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Username { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    public static User Create(string username, string email, string passwordHash)
        => new User { Username = username, Email = email, PasswordHash = passwordHash };
}
