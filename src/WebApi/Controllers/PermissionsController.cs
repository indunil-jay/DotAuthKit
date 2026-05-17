using Application.Permissions;
using Domain.Permissions;
using Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/permissions")]
[Authorize]
public sealed class PermissionsController(IPermissionRepository permissionRepository) : ApiController
{
    [HttpGet]
    [HasPermission(Permissions.Roles.View)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        IReadOnlyList<Permission> permissions = await permissionRepository.GetAllAsync(cancellationToken);

        var response = permissions.Select(p => new
        {
            p.Id,
            p.Name,
            p.Module,
            p.Action,
            p.Description
        });

        return Ok(response);
    }
}
