using Domain.Users;

namespace Infrastructure.Persistence.Specifications.Users;

internal sealed class UserByIdSpecification : Specification<User>
{
    public UserByIdSpecification(Guid id)
        : base(user => user.Id == id)
    {
    }
}
