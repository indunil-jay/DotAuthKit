using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Databases;

public interface IApplicationDbContext
{
     DbSet<User> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

