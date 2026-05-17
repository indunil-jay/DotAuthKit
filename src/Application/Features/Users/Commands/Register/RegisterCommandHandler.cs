using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Users;
using SharedKernel;

namespace Application.Features.Users.Commands.Register;

internal sealed class RegisterCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var user = User.CreateNew(command.Name, command.Email, command.Password);

        userRepository.Add(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(user.Id);
    }
}
