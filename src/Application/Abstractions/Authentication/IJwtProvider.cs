using Domain.Users;

namespace Application.Abstractions.Authentication;

public interface IJwtProvider
{
    Task<string> GenerateTokenAsync(User user, CancellationToken cancellationToken = default);
}
