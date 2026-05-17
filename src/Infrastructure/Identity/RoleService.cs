using System.Security.Claims;
using Application.Abstractions.Authorization;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Identity;

internal sealed class RoleService(
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager)
    : IRoleService
{
    private const string PermissionClaimType = "permissions";

    public async Task<Result<RoleDto>> CreateRoleAsync(
        string name,
        string? description,
        CancellationToken cancellationToken = default)
    {
        if (await roleManager.RoleExistsAsync(name))
        {
            return Result.Failure<RoleDto>(RoleErrors.AlreadyExists(name));
        }

        var role = new ApplicationRole(name) { Description = description };

        IdentityResult result = await roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            return Result.Failure<RoleDto>(RoleErrors.IdentityFailure(result.Errors));
        }

        return Result.Success(new RoleDto(role.Id, role.Name!, role.Description, []));
    }

    public async Task<Result<IReadOnlyList<RoleDto>>> GetAllRolesAsync(
        CancellationToken cancellationToken = default)
    {
        List<ApplicationRole> roles = await roleManager.Roles.ToListAsync(cancellationToken);

        var roleDtos = new List<RoleDto>(roles.Count);

        foreach (ApplicationRole role in roles)
        {
            IList<Claim> claims = await roleManager.GetClaimsAsync(role);
            var permissions = claims
                .Where(c => c.Type == PermissionClaimType)
                .Select(c => c.Value)
                .ToList();

            roleDtos.Add(new RoleDto(role.Id, role.Name!, role.Description, permissions));
        }

        return Result.Success<IReadOnlyList<RoleDto>>(roleDtos.AsReadOnly());
    }

    public async Task<Result<IReadOnlyList<string>>> GetRolePermissionsAsync(
        string roleName,
        CancellationToken cancellationToken = default)
    {
        ApplicationRole? role = await roleManager.FindByNameAsync(roleName);

        if (role is null)
        {
            return Result.Failure<IReadOnlyList<string>>(RoleErrors.NotFound(roleName));
        }

        IList<Claim> claims = await roleManager.GetClaimsAsync(role);
        IReadOnlyList<string> permissions = claims
            .Where(c => c.Type == PermissionClaimType)
            .Select(c => c.Value)
            .ToList()
            .AsReadOnly();

        return Result.Success(permissions);
    }

    public async Task<Result> AddPermissionToRoleAsync(
        string roleName,
        string permission,
        CancellationToken cancellationToken = default)
    {
        ApplicationRole? role = await roleManager.FindByNameAsync(roleName);

        if (role is null)
        {
            return Result.Failure(RoleErrors.NotFound(roleName));
        }

        IList<Claim> existing = await roleManager.GetClaimsAsync(role);

        if (existing.Any(c => c.Type == PermissionClaimType && c.Value == permission))
        {
            return Result.Success();
        }

        IdentityResult result = await roleManager.AddClaimAsync(
            role,
            new Claim(PermissionClaimType, permission));

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(RoleErrors.IdentityFailure(result.Errors));
    }

    public async Task<Result> RemovePermissionFromRoleAsync(
        string roleName,
        string permission,
        CancellationToken cancellationToken = default)
    {
        ApplicationRole? role = await roleManager.FindByNameAsync(roleName);

        if (role is null)
        {
            return Result.Failure(RoleErrors.NotFound(roleName));
        }

        IdentityResult result = await roleManager.RemoveClaimAsync(
            role,
            new Claim(PermissionClaimType, permission));

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(RoleErrors.IdentityFailure(result.Errors));
    }

    public async Task<Result> AssignRoleToUserAsync(
        Guid userId,
        string roleName,
        CancellationToken cancellationToken = default)
    {
        ApplicationUser? appUser = await userManager.FindByIdAsync(userId.ToString());

        if (appUser is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        if (!await roleManager.RoleExistsAsync(roleName))
        {
            return Result.Failure(RoleErrors.NotFound(roleName));
        }

        IdentityResult result = await userManager.AddToRoleAsync(appUser, roleName);

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(RoleErrors.IdentityFailure(result.Errors));
    }

    public async Task<Result> RemoveRoleFromUserAsync(
        Guid userId,
        string roleName,
        CancellationToken cancellationToken = default)
    {
        ApplicationUser? appUser = await userManager.FindByIdAsync(userId.ToString());

        if (appUser is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        IdentityResult result = await userManager.RemoveFromRoleAsync(appUser, roleName);

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(RoleErrors.IdentityFailure(result.Errors));
    }

    public async Task<Result<IReadOnlyList<string>>> GetUserRolesAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        ApplicationUser? appUser = await userManager.FindByIdAsync(userId.ToString());

        if (appUser is null)
        {
            return Result.Failure<IReadOnlyList<string>>(UserErrors.NotFound);
        }

        IList<string> roles = await userManager.GetRolesAsync(appUser);

        return Result.Success<IReadOnlyList<string>>(roles.ToList().AsReadOnly());
    }
}
