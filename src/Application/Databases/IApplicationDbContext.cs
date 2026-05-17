using Domain.Permissions;
using Microsoft.EntityFrameworkCore;

namespace Application.Databases;

public interface IApplicationDbContext
{
    DbSet<Permission> Permissions { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
