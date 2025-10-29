namespace AuthService.Domain.ValueObjects;

using BCrypt.Net;

public class Password
{
    public string Hash { get; private set; }

    private Password(string hash)
    {
        Hash = hash;
    }

    public static Password Create(string password)
    {
        var hash = BCrypt.HashPassword(password);

        return new Password(hash);
    }

    public bool Verify(string hash, string password)
    {
        return BCrypt.Verify(password, hash);
    }
}