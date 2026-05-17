using System.Security.Claims;
using Application.Databases;
using Application.Permissions;
using Domain.Permissions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity;

public sealed class DataSeeder(
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager,
    IApplicationDbContext dbContext)
{
    private const string PermissionClaimType = "permissions";
    private const string SeedAdminEmail = "admin@dotauthkit.com";
    private const string SeedAdminInitialCredential = "Admin@1234";

    public async Task SeedAsync()
    {
        await SeedPermissionsAsync();
        await SeedRolesAsync();
        await SeedAdminUserAsync();
    }

    private async Task SeedPermissionsAsync()
    {
        foreach (string permissionName in Permissions.All)
        {
            string[] parts = permissionName.Split(':');
            if (parts.Length != 2)
            {
                continue;
            }

            string module = parts[0];
            string action = parts[1];

            bool exists = await dbContext.Permissions
                .AnyAsync(p => p.Module == module && p.Action == action);

            if (!exists)
            {
                dbContext.Permissions.Add(Permission.Create(module, action));
            }
        }

        await dbContext.SaveChangesAsync();
    }

    private async Task SeedRolesAsync()
    {
        await EnsureRoleWithPermissionsAsync("Admin", "Full system access", Permissions.All);

        await EnsureRoleWithPermissionsAsync("User", "Standard user access", [
            Permissions.Todos.View,
            Permissions.Todos.Create,
            Permissions.Todos.Update,
            Permissions.Todos.Delete,
        ]);
    }

    private async Task EnsureRoleWithPermissionsAsync(
        string roleName,
        string description,
        IReadOnlyList<string> permissions)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var role = new ApplicationRole(roleName) { Description = description };
            await roleManager.CreateAsync(role);
        }

        ApplicationRole? existingRole = await roleManager.FindByNameAsync(roleName);
        if (existingRole is null)
        {
            return;
        }

        IList<Claim> existingClaims = await roleManager.GetClaimsAsync(existingRole);
        var existingPermissions = existingClaims
            .Where(c => c.Type == PermissionClaimType)
            .Select(c => c.Value)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (string permission in permissions)
        {
            if (!existingPermissions.Contains(permission))
            {
                await roleManager.AddClaimAsync(existingRole, new Claim(PermissionClaimType, permission));
            }
        }
    }

    private async Task SeedAdminUserAsync()
    {
        if (await userManager.FindByEmailAsync(SeedAdminEmail) is not null)
        {
            return;
        }

        var admin = new ApplicationUser
        {
            Id = Guid.CreateVersion7(),
            Name = "System Admin",
            Email = SeedAdminEmail,
            UserName = SeedAdminEmail,
        };

        await userManager.CreateAsync(admin, SeedAdminInitialCredential);
        await userManager.AddToRoleAsync(admin, "Admin");
    }
}
