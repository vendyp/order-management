using OrderManagementApi.Domain.Entities;
using Shouldly;

namespace OrderManagementApi.UnitTests.Entities;

public class RoleScopeTests
{
    [Fact]
    public void RoleScope_Ctor_Should_Be_Correct()
    {
        const string roleId = "test";
        const string name = "test123";

        var roleScope = new RoleScope(roleId, name);

        roleScope.RoleId.ShouldBe(roleId);
        roleScope.Name.ShouldBe(name);
        roleScope.RoleScopeId.ShouldNotBe(default);
    }
}