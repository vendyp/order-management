using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Entities;
using Shouldly;

namespace OrderManagementApi.UnitTests.Entities;

public class UserTokenTests
{
    [Fact]
    public void Ctor_UserToken_ShouldBe_Ok()
    {
        var userToken = new UserToken();
        userToken.IsUsed.ShouldBeFalse();
        userToken.UserTokenId.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public void Ctor_UserToken_GetType_Must_Satisfied_Design()
    {
        var type = typeof(UserToken);
        type.BaseType.ShouldNotBeNull();
        type.BaseType.ShouldBe(typeof(BaseEntity));
        typeof(IEntity).IsAssignableFrom(type).ShouldBeTrue();
        type.IsSealed.ShouldBeTrue();
    }

    [Fact]
    public void Ctor_UserToken_UseUserToken_ShouldBe_Ok()
    {
        var userToken = new UserToken();
        userToken.IsUsed.ShouldBeFalse();
        userToken.UsedAt.ShouldBe(null);
        var dt = DateTime.Now;
        userToken.UseUserToken(dt);
        userToken.IsUsed.ShouldBeTrue();
        userToken.UsedAt.ShouldNotBe(null);
    }
}