namespace AuthService.Domain.ValueObjects;

using BCrypt.Net;

public class Password
{
    public string Hash { get; private set; }

    private Password(string hash)
    {
        Hash = hash;
    }

    // TODO настроить валидацию
    public static Password FromPlainPassword(string plainPassword)
    {
        if (string.IsNullOrWhiteSpace(plainPassword))
            throw new ArgumentException("Password cannot be empty");

        if (plainPassword.Length < 6)
            throw new ArgumentException("Password too short");

        var hash = BCrypt.HashPassword(plainPassword);
        return new Password(hash);
    }
    
    public static Password Create(string password)
    {
        var hash = BCrypt.HashPassword(password);

        return new Password(hash);
    }

    public bool Verify(string password)
    {
        return BCrypt.Verify(password, Hash);
    }
    
    public override string ToString() => Hash;

}