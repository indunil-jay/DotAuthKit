using Application.Abstractions.Authorization;
using Application.Permissions;
using Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebApi.Controllers;

[Route("api/roles")]
[Authorize]
public sealed class RolesController(IRoleService roleService) : ApiController
{
    [HttpGet]
    [HasPermission(Permissions.Roles.View)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        Result<IReadOnlyList<RoleDto>> result = await roleService.GetAllRolesAsync(cancellationToken);
        return HandleResult(result);
    }

    [HttpPost]
    [HasPermission(Permissions.Roles.Create)]
    public async Task<IActionResult> Create(
        [FromBody] CreateRoleRequest request,
        CancellationToken cancellationToken)
    {
        Result<RoleDto> result = await roleService.CreateRoleAsync(
            request.Name,
            request.Description,
            cancellationToken);

        return HandleResult(result);
    }

    [HttpGet("{roleName}/permissions")]
    [HasPermission(Permissions.Roles.View)]
    public async Task<IActionResult> GetPermissions(
        string roleName,
        CancellationToken cancellationToken)
    {
        Result<IReadOnlyList<string>> result = await roleService.GetRolePermissionsAsync(
            roleName,
            cancellationToken);

        return HandleResult(result);
    }

    [HttpPost("{roleName}/permissions")]
    [HasPermission(Permissions.Roles.AssignPermissions)]
    public async Task<IActionResult> AddPermission(
        string roleName,
        [FromBody] RolePermissionRequest request,
        CancellationToken cancellationToken)
    {
        Result result = await roleService.AddPermissionToRoleAsync(
            roleName,
            request.Permission,
            cancellationToken);

        return HandleResult(result);
    }

    [HttpDelete("{roleName}/permissions/{permission}")]
    [HasPermission(Permissions.Roles.AssignPermissions)]
    public async Task<IActionResult> RemovePermission(
        string roleName,
        string permission,
        CancellationToken cancellationToken)
    {
        Result result = await roleService.RemovePermissionFromRoleAsync(
            roleName,
            permission,
            cancellationToken);

        return HandleResult(result);
    }

    [HttpPost("{roleName}/users/{userId:guid}")]
    [HasPermission(Permissions.Users.AssignRoles)]
    public async Task<IActionResult> AssignRoleToUser(
        string roleName,
        Guid userId,
        CancellationToken cancellationToken)
    {
        Result result = await roleService.AssignRoleToUserAsync(userId, roleName, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{roleName}/users/{userId:guid}")]
    [HasPermission(Permissions.Users.AssignRoles)]
    public async Task<IActionResult> RemoveRoleFromUser(
        string roleName,
        Guid userId,
        CancellationToken cancellationToken)
    {
        Result result = await roleService.RemoveRoleFromUserAsync(userId, roleName, cancellationToken);
        return HandleResult(result);
    }
}

public sealed record CreateRoleRequest(string Name, string? Description);
public sealed record RolePermissionRequest(string Permission);
