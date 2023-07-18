using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Enums;
using OrderManagementApi.UnitTests.Builders;
using OrderManagementApi.WebApi.Endpoints.UserManagement;
using Moq;
using Shouldly;

namespace OrderManagementApi.UnitTests.Endpoints.UserManagement;

public class SetUserInActiveUnitTests
{
    [Fact]
    public async Task SetUserInActive_Should_Be_Correct()
    {
        var dbContext = DbContextBuilder.Create();

        User? user = null;

        var listUserDataTest = new List<User>
        {
            new() { UserId = Guid.Empty, StatusRecord = StatusRecord.Active }
        };

        dbContext.Setup(e => e.AttachEntity(It.IsAny<User>()))
            .Callback<User>(entity =>
            {
                user = entity; // set reference
                //first time passed should be Active;
                user.StatusRecord.ShouldBe(StatusRecord.Active);
            });

        dbContext.Setup(listUserDataTest);

        var setUserInActive = new SetUserInActive(dbContext.Object);
        _ = await setUserInActive.HandleAsync(new SetUserInActiveRequest { UserId = Guid.Empty },
            CancellationToken.None);

        //class ref
        user.ShouldNotBeNull();
        user.StatusRecord.ShouldBe(StatusRecord.InActive);
    }
}