namespace Auth.Application.Contracts;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string hash, string password);
}
