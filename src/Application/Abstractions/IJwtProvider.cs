using Domain.Users;

namespace Application.Abstractions;

public interface IJwtProvider
{
    string GenerateToken(User user);
}
