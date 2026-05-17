using Application.Abstractions.Messaging;
using Application.Databases;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Users.Commands.Login;

internal sealed class LogginCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<LogginCommand, string>
{
    public async Task<Result<string>> Handle(LogginCommand command, CancellationToken cancellationToken)
    {
        User? user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == command.Email, cancellationToken);

        if (user is null || user.Password != command.Password)
        {
            return Result.Failure<string>(Error.Problem("Auth.InvalidCredentials", "Invalid email or password."));
        }

        return Result.Success($"mock-jwt-token-for-{user.Id}");
    }
}
