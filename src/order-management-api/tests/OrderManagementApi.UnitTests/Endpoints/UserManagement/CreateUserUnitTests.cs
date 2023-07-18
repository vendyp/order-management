using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Models;
using OrderManagementApi.UnitTests.Builders;
using OrderManagementApi.WebApi.Endpoints.UserManagement;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Moq;

namespace OrderManagementApi.UnitTests.Endpoints.UserManagement;

public class CreateUserUnitTests
{
    [Fact]
    public async Task CreateUser_NewUser_That_Passed_ShouldBe_Correct()
    {
        //setup
        var dbContext = DbContextBuilder.Create();
        var userService = UserServiceBuilder.Create();
        var rng = RngBuilder.Create();
        var salter = SalterBuilder.Create();

        var request = new CreateUserRequest
        {
            Username = "admin",
            Password = "Qwerty@1234",
            Fullname = "Administrator",
            EmailAddress = "test@test.com"
        };

        //setup rng return abcdef
        const string salt = "abcdef";
        rng.Setup(e => e.Generate(It.IsAny<int>(), It.IsAny<bool>())).Returns(salt);

        //setup salter return aaaaaa, when passed salt and request of password
        const string hash = "aaaaaa";
        salter.Setup(e => e.Hash(salt, request.Password)).Returns(hash);

        //validates callback user that passed to insert, must satisfied those requirements
        dbContext.Setup(e => e.Insert(It.IsAny<User>()))
            .Callback<User>((entity) =>
            {
                entity.Username.ShouldBe(request.Username);
                entity.NormalizedUsername.ShouldBe(request.Username.ToUpper());
                entity.FullName.ShouldBe(request.Fullname);
                entity.Salt.ShouldBe(salt);
                entity.Password.ShouldNotBeNullOrWhiteSpace();
                entity.Password.ShouldBe(hash);
            });

        var createUser = new CreateUser(dbContext.Object,
            userService.Object,
            rng.Object,
            salter.Object);

        _ = await createUser.HandleAsync(request, CancellationToken.None);

        dbContext.Verify(e => e.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    public static IEnumerable<object[]> GetInvalidRequests()
    {
        yield return new object[]
        {
            new CreateUserRequest
            {
                Username = string.Empty,
                Password = string.Empty,
                Fullname = string.Empty,
                EmailAddress = null
            }
        };

        yield return new object[]
        {
            new CreateUserRequest
            {
                Username = string.Empty,
                Password = string.Empty,
                Fullname = string.Empty,
                EmailAddress = "lorep ipsum"
            }
        };

        yield return new object[]
        {
            new CreateUserRequest
            {
                Username = string.Empty,
                Password = string.Empty,
                Fullname = string.Empty,
                EmailAddress = "test@test.com"
            }
        };
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public async Task CreateUser_Given_InvalidRequest_ShouldReturn_BadRequest(CreateUserRequest request)
    {
        //setup
        var dbContext = DbContextBuilder.Create();
        var userService = UserServiceBuilder.Create();
        var rng = RngBuilder.Create();
        var salter = SalterBuilder.Create();

        var createUser = new CreateUser(dbContext.Object,
            userService.Object,
            rng.Object,
            salter.Object);

        var result = await createUser.HandleAsync(request, CancellationToken.None);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();
    }

    [Fact]
    public async Task CreateUser_Given_CorrectRequest_With_Already_Same_Username_ShouldReturn_BadRequest()
    {
        //setup
        var dbContext = DbContextBuilder.Create();
        var userService = UserServiceBuilder.Create();
        var rng = RngBuilder.Create();
        var salter = SalterBuilder.Create();

        var request = new CreateUserRequest
        {
            Username = "admin",
            Password = "Test@12345",
            Fullname = "Super Administrator",
            EmailAddress = "test@test.com"
        };
        userService.Setup(e => e.IsUsernameExistAsync(request.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var createUser = new CreateUser(dbContext.Object,
            userService.Object,
            rng.Object,
            salter.Object);


        // Act
        var result = await createUser.HandleAsync(request);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();
        var err = (actual.Value as Error);
        err.ShouldNotBeNull();
        err.Message.ShouldBe("Username already exists");
    }
}