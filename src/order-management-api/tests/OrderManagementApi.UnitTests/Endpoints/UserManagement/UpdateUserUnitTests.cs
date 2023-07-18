using OrderManagementApi.Shared.Abstractions.Models;
using OrderManagementApi.UnitTests.Builders;
using OrderManagementApi.WebApi.Endpoints.UserManagement;
using Microsoft.AspNetCore.Mvc;
using Shouldly;

namespace OrderManagementApi.UnitTests.Endpoints.UserManagement;

public class UpdateUserUnitTests
{
    public static IEnumerable<object[]> GetInvalidRequests()
    {
        yield return new object[]
        {
            new UpdateUserRequest
            {
                UserId = Guid.Empty,
                UpdateUserRequestPayload = new UpdateUserRequestPayload()
            }
        };

        yield return new object[]
        {
            new UpdateUserRequest
            {
                UserId = Guid.Empty,
                UpdateUserRequestPayload = new UpdateUserRequestPayload
                {
                    Fullname = string.Empty
                }
            }
        };
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public async Task UpdateUser_Given_InvalidRequest_ShouldReturn_BadRequest(UpdateUserRequest request)
    {
        var updateUser = new UpdateUser(DbContextBuilder.Create().Object);

        // Act
        var result = await updateUser.HandleAsync(request);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();
    }
}