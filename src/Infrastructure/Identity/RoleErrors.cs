using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Infrastructure.Identity;

internal static class RoleErrors
{
    public static Error NotFound(string roleName) =>
        Error.NotFound("Role.NotFound", $"Role '{roleName}' was not found.");

    public static Error AlreadyExists(string roleName) =>
        Error.Conflict("Role.AlreadyExists", $"Role '{roleName}' already exists.");

    public static Error IdentityFailure(IEnumerable<IdentityError> errors) =>
        Error.Problem("Role.IdentityFailure", string.Join("; ", errors.Select(e => e.Description)));
}
