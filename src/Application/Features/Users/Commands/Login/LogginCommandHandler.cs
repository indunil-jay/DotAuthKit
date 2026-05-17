using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Domain.Users;
using SharedKernel;

namespace Application.Features.Users.Commands.Login;

internal sealed class LogginCommandHandler(
    IIdentityService identityService,
    IJwtProvider jwtProvider)
    : ICommandHandler<LogginCommand, string>
{
    public async Task<Result<string>> Handle(LogginCommand command, CancellationToken cancellationToken)
    {
        Result<User> authResult = await identityService.AuthenticateAsync(
            command.Email,
            command.Password,
            cancellationToken);

        if (authResult.IsFailure)
        {
            return Result.Failure<string>(authResult.Error);
        }

        string token = await jwtProvider.GenerateTokenAsync(authResult.Value, cancellationToken);

        return Result.Success(token);
    }
}
