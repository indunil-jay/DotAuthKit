using SharedKernel;

namespace Domain.Users;

public sealed class User : Entity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    private User() { }

    public static User Create(Guid id, string name, string email)
    {
        return new User { Id = id, Name = name, Email = email };
    }
}
