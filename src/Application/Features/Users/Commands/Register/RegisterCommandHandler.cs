using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Features.Users.Commands.Register;

internal sealed class RegisterCommandHandler(IIdentityService identityService)
    : ICommandHandler<RegisterCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        return await identityService.CreateUserAsync(
            command.Name,
            command.Email,
            command.Password,
            cancellationToken);
    }
}
