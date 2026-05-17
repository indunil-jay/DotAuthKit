using Application.Abstractions.Messaging;

namespace Application.Features.Users.Commands.Login;

public record LogginCommand(string Email, string Password): ICommand<string>;
