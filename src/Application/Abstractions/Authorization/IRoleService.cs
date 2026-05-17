using SharedKernel;

namespace Application.Abstractions.Authorization;

public interface IRoleService
{
    Task<Result<RoleDto>> CreateRoleAsync(
        string name,
        string? description,
        CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<RoleDto>>> GetAllRolesAsync(
        CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<string>>> GetRolePermissionsAsync(
        string roleName,
        CancellationToken cancellationToken = default);

    Task<Result> AddPermissionToRoleAsync(
        string roleName,
        string permission,
        CancellationToken cancellationToken = default);

    Task<Result> RemovePermissionFromRoleAsync(
        string roleName,
        string permission,
        CancellationToken cancellationToken = default);

    Task<Result> AssignRoleToUserAsync(
        Guid userId,
        string roleName,
        CancellationToken cancellationToken = default);

    Task<Result> RemoveRoleFromUserAsync(
        Guid userId,
        string roleName,
        CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<string>>> GetUserRolesAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
