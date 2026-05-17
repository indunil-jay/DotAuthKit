using Application.Abstractions.Messaging;

namespace Application.Features.Users.Commands.Register;

public record RegisterCommand(
    string Name,
    string Email,
    string Password) : ICommand<Guid>;
