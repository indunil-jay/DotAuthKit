using Application.Abstractions.Authorization;
using Application.Permissions;
using Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebApi.Controllers;

[Route("api/users")]
[Authorize]
public sealed class UsersController(IRoleService roleService) : ApiController
{
    [HttpGet("{userId:guid}/roles")]
    [HasPermission(Permissions.Users.View)]
    public async Task<IActionResult> GetUserRoles(Guid userId, CancellationToken cancellationToken)
    {
        Result<IReadOnlyList<string>> result = await roleService.GetUserRolesAsync(userId, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{userId:guid}/roles")]
    [HasPermission(Permissions.Users.AssignRoles)]
    public async Task<IActionResult> AssignRole(
        Guid userId,
        [FromBody] AssignRoleRequest request,
        CancellationToken cancellationToken)
    {
        Result result = await roleService.AssignRoleToUserAsync(userId, request.RoleName, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{userId:guid}/roles/{roleName}")]
    [HasPermission(Permissions.Users.AssignRoles)]
    public async Task<IActionResult> RemoveRole(
        Guid userId,
        string roleName,
        CancellationToken cancellationToken)
    {
        Result result = await roleService.RemoveRoleFromUserAsync(userId, roleName, cancellationToken);
        return HandleResult(result);
    }
}

public sealed record AssignRoleRequest(string RoleName);
