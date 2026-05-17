using Application.Abstractions.Authentication;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Infrastructure.Identity;

internal sealed class IdentityService(UserManager<ApplicationUser> userManager)
    : IIdentityService
{
    public async Task<Result<Guid>> CreateUserAsync(
        string name,
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        if (await userManager.FindByEmailAsync(email) is not null)
        {
            return Result.Failure<Guid>(UserErrors.DuplicateEmail(email));
        }

        var appUser = new ApplicationUser
        {
            Id = Guid.CreateVersion7(),
            Name = name,
            Email = email,
            UserName = email,
        };

        IdentityResult result = await userManager.CreateAsync(appUser, password);

        if (!result.Succeeded)
        {
            return Result.Failure<Guid>(UserErrors.IdentityFailure(
                string.Join("; ", result.Errors.Select(e => e.Description))));
        }

        return Result.Success(appUser.Id);
    }

    public async Task<Result<User>> AuthenticateAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        ApplicationUser? appUser = await userManager.FindByEmailAsync(email);

        if (appUser is null)
        {
            return Result.Failure<User>(UserErrors.InvalidCredentials);
        }

        bool passwordValid = await userManager.CheckPasswordAsync(appUser, password);

        if (!passwordValid)
        {
            return Result.Failure<User>(UserErrors.InvalidCredentials);
        }

        return Result.Success(User.Create(appUser.Id, appUser.Name, appUser.Email!));
    }
}
