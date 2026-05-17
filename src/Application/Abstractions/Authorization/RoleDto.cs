namespace Application.Abstractions.Authorization;

public sealed record RoleDto(
    Guid Id,
    string Name,
    string? Description,
    IReadOnlyList<string> Permissions);
