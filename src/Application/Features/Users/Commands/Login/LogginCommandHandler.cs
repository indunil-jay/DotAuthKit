using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Users;
using SharedKernel;

namespace Application.Features.Users.Commands.Login;

internal sealed class LogginCommandHandler(
    IUserRepository userRepository,
    IJwtProvider jwtProvider
    )
    : ICommandHandler<LogginCommand, string>
{
    public async Task<Result<string>> Handle(LogginCommand command, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByEmailAsync(command.Email, cancellationToken);

        if (user is null || user.Password != command.Password)
        {
            return Result.Failure<string>(Error.Problem("Auth.InvalidCredentials", "Invalid email or password."));
        }

        string token = jwtProvider.GenerateToken(user);

        return Result.Success(token);
    }
}
