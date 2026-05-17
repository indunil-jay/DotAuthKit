using SharedKernel;

namespace Domain.Users;

public sealed class User : Entity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }

    public User() { }

    private User(Guid id, string name, string email, string password)
    {
        Id = id;
        Name = name;
        Email = email;
        Password = password;
    }

    public static User CreateNew(string name, string email, string passwordHash)
    {
        return new User(Guid.CreateVersion7(), name, email, passwordHash);
    }
}
