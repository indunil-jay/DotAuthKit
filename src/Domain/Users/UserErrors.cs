using SharedKernel;

namespace Domain.Users;

public static class UserErrors
{
    public static readonly Error NotFound =
        Error.NotFound("User.NotFound", "User not found.");

    public static readonly Error InvalidCredentials =
        Error.Problem("User.InvalidCredentials", "Invalid email or password.");

    public static Error DuplicateEmail(string email) =>
        Error.Conflict("User.DuplicateEmail", $"A user with email '{email}' already exists.");

    public static Error IdentityFailure(string description) =>
        Error.Problem("User.IdentityFailure", description);
}
