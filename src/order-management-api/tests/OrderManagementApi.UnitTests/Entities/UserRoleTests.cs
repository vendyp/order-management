using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Entities;
using Shouldly;

namespace OrderManagementApi.UnitTests.Entities;

public class UserRoleTests
{
    [Fact]
    public void Ctor_UserRole_ShouldBeOk()
    {
        var userRole = new UserRole();

        userRole.UserId.ShouldBe(Guid.Empty);
        userRole.RoleId.ShouldBe(string.Empty);
    }

    [Fact]
    public void Ctor_UserRole_GetType_Must_Satisfied_Design()
    {
        var type = typeof(UserRole);
        type.BaseType.ShouldNotBeNull();
        type.IsSealed.ShouldBeTrue();
        typeof(IEntity).IsAssignableFrom(type).ShouldBeTrue();
    }
}