using Application.Databases;
using Domain.Permissions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

internal sealed class PermissionRepository(IApplicationDbContext dbContext) : IPermissionRepository
{
    public async Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        string[] parts = name.Split(':');
        if (parts.Length != 2)
        {
            return null;
        }

        return await dbContext.Permissions
            .FirstOrDefaultAsync(
                p => p.Module == parts[0] && p.Action == parts[1],
                cancellationToken);
    }

    public async Task<IReadOnlyList<Permission>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Permissions
            .OrderBy(p => p.Module)
            .ThenBy(p => p.Action)
            .ToListAsync(cancellationToken);

    public bool Exists(string name)
    {
        string[] parts = name.Split(':');
        if (parts.Length != 2)
        {
            return false;
        }

        return dbContext.Permissions
            .Any(p => p.Module == parts[0] && p.Action == parts[1]);
    }

    public void Add(Permission permission) => dbContext.Permissions.Add(permission);
}
