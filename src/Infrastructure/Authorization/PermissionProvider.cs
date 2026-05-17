using System.Security.Claims;

namespace Infrastructure.Authorization;

internal sealed class PermissionProvider
{
    private const string PermissionClaimType = "permissions";

    public HashSet<string> GetPermissions(ClaimsPrincipal principal)
    {
        return principal.Claims
            .Where(c => c.Type == PermissionClaimType)
            .Select(c => c.Value)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }
}
