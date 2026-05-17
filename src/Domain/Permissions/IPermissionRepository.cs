namespace Domain.Permissions;

public interface IPermissionRepository
{
    Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Permission>> GetAllAsync(CancellationToken cancellationToken = default);
    bool Exists(string name);
    void Add(Permission permission);
}
