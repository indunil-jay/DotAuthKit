using Application.Abstractions.Messaging;
using Application.Features.Users.Commands.Login;
using Application.Features.Users.Commands.Register;
using Application.Features.Users.Requests;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebApi.Controllers;

[Route("api/authentication")]
public sealed class AuthController : ApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        [FromServices] ICommandHandler<RegisterCommand, Guid> registerHandler,
        CancellationToken cancellationToken)
    {
        RegisterCommand command = new(
            request.Name,
            request.Email,
            request.Password);

        Result<Guid> result = await registerHandler.Handle(command, cancellationToken);

        return HandleResult(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LogginRequest request,
        [FromServices] ICommandHandler<LogginCommand, string> loginHandler,
        CancellationToken cancellationToken)
    {
        LogginCommand command = new(
            request.Email,
            request.Password);

        Result<string> result = await loginHandler.Handle(command, cancellationToken);

        return HandleResult(result);
    }
}
