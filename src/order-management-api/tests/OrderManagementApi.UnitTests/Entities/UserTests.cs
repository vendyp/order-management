using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Entities;
using Shouldly;

namespace OrderManagementApi.UnitTests.Entities;

public class UserTests
{
    [Fact]
    public void Ctor_User_ShouldBe_Ok()
    {
        var user = new User();
        user.UserId.ShouldNotBe(Guid.Empty);
        user.UserTokens.ShouldNotBeNull();
        user.UserRoles.ShouldNotBeNull();
        
        const string s = "test";
        user.Username = s;
        user.NormalizedUsername = s;
        user.NormalizedUsername.ShouldBe(s.ToUpper());
    }

    [Fact]
    public void Ctor_User_GetType_Must_Satisfied_Design()
    {
        var type = typeof(User);
        type.BaseType.ShouldNotBeNull();
        type.BaseType.ShouldBe(typeof(BaseEntity));
        typeof(IEntity).IsAssignableFrom(type).ShouldBeTrue();
        type.IsSealed.ShouldBeTrue();
    }

    [Fact]
    public void Ctor_User_UpdatePassword_ShouldBe_Ok()
    {
        const string salt = "abcd";
        const string pass = "efgh";

        var user = new User();
        user.Salt.ShouldBeNull();
        user.Password.ShouldBeNull();
        user.UpdatePassword(salt, pass);
        user.Salt.ShouldNotBeNull();
        user.Password.ShouldNotBeNull();
        user.Salt.ShouldBe(salt);
        user.Password.ShouldBe(pass);
    }

    [Fact]
    public void Ctor_User_UpdatePassword_ShouldBe_Exceptions_When_Pass_Invalid_Args()
    {
        var user = new User();
        user.Salt.ShouldBeNull();
        user.Password.ShouldBeNull();
        Should.Throw<ArgumentNullException>(() => { user.UpdatePassword(string.Empty, ""); });
    }
}