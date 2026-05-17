using SharedKernel;

namespace Domain.Permissions;

public sealed class Permission : Entity
{
    public string Module { get; private set; } = string.Empty;
    public string Action { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public string Name => $"{Module}:{Action}";

    private Permission() { }

    public static Permission Create(string module, string action, string? description = null)
    {
        return new Permission
        {
            Id = Guid.CreateVersion7(),
            Module = module,
            Action = action,
            Description = description
        };
    }
}
