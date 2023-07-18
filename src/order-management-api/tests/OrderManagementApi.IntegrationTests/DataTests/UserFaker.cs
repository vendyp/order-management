using Bogus;
using OrderManagementApi.Domain.Entities;

namespace OrderManagementApi.IntegrationTests.DataTests;

public sealed class UserFaker : Faker<User>
{
    public UserFaker()
    {
        RuleFor(o => o.UserId, f => Guid.NewGuid());
        RuleFor(o => o.Username, f => f.Internet.UserName());
        RuleFor(o => o.NormalizedUsername, (f, u) => u.Username);
        RuleFor(e => e.FullName, f => f.Name.FullName());
        RuleFor(e => e.Email, (f, u) => f.Internet.Email(u.FullName));
    }
}