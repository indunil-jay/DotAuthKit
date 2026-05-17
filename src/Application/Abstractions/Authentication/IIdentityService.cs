using Domain.Users;
using SharedKernel;

namespace Application.Abstractions.Authentication;

public interface IIdentityService
{
    Task<Result<Guid>> CreateUserAsync(
        string name,
        string email,
        string password,
        CancellationToken cancellationToken = default);

    Task<Result<User>> AuthenticateAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);
}
