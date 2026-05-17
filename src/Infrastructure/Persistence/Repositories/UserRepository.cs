using Domain.Users;
using Infrastructure.Persistence.Specifications.Users;

namespace Infrastructure.Persistence.Repositories;

internal sealed class UserRepository(ApplicationDbContext dbContext)
    : Repository<User>(dbContext), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await GetFirstOrDefaultAsync(
            new UserByEmailSpecification(email),
            cancellationToken);
    }
}
