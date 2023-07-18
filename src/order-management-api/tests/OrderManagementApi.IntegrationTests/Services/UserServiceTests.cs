using OrderManagementApi.Core.Abstractions;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.IntegrationTests.DataTests;
using OrderManagementApi.Shared.Abstractions.Databases;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace OrderManagementApi.IntegrationTests.Services;

[Collection(nameof(ServiceFixture))]
public class UserServiceTests
{
    public static readonly Random Random = new();
    public ServiceFixture ServiceFixture { get; }
    public List<User> Users { get; }

    public UserServiceTests(ServiceFixture serviceFixture)
    {
        ServiceFixture = serviceFixture;

        var userFaker = new UserFaker();
        Users = new List<User>();
        Users.AddRange(userFaker.Generate(20));

        var dbContext = ServiceFixture.ServiceProvider.GetRequiredService<IDbContext>();
        dbContext.Set<User>().AddRange(Users);
        dbContext.SaveChangesAsync().GetAwaiter().GetResult();
    }

    [Fact]
    public async Task UserService_GetUserById_Should_Return_Correct()
    {
        //get random
        var i = Random.Next(0, Users.Count - 1);

        var service = ServiceFixture.ServiceProvider.GetRequiredService<IUserService>();
        var userFake = Users[i];
        var user = await service.GetUserByIdAsync(userFake.UserId, CancellationToken.None);

        user.ShouldNotBeNull();
        userFake.ShouldBeEquivalentTo(user);
    }

    [Fact]
    public async Task UserService_GetUserByUsername_Should_Return_Correct()
    {
        //get random
        var i = Random.Next(0, Users.Count - 1);

        var service = ServiceFixture.ServiceProvider.GetRequiredService<IUserService>();
        var userFake = Users[i];
        var user = await service.GetUserByUsernameAsync(userFake.Username, CancellationToken.None);

        user.ShouldNotBeNull();
        userFake.ShouldBeEquivalentTo(user);
    }
}