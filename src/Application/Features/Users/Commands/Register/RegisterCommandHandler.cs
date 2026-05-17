using Application.Abstractions.Messaging;
using Application.Databases;
using Domain.Users;
using SharedKernel;

namespace Application.Features.Users.Commands.Register;

internal sealed class RegisterCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<RegisterCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var user = User.CreateNew(command.Name, command.Email, command.Password);

        dbContext.Users.Add(user);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(user.Id);
    }
}
