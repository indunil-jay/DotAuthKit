using Domain.Users;

namespace Infrastructure.Persistence.Specifications.Users;

internal sealed class UserByEmailSpecification : Specification<User>
{
    public UserByEmailSpecification(string email)
        : base(user => user.Email == email)
    {
    }
}
