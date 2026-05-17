using Domain.Users;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence.Repositories;

internal sealed class UserRepository(UserManager<ApplicationUser> userManager) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        ApplicationUser? appUser = await userManager.FindByIdAsync(id.ToString());
        return appUser is null ? null : User.Create(appUser.Id, appUser.Name, appUser.Email ?? string.Empty);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        ApplicationUser? appUser = await userManager.FindByEmailAsync(email);
        return appUser is null ? null : User.Create(appUser.Id, appUser.Name, appUser.Email ?? string.Empty);
    }
}
