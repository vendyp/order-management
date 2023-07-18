using OrderManagementApi.Domain.Entities;
using Shouldly;

namespace OrderManagementApi.UnitTests.Entities;

public class RoleTests
{
    [Fact]
    public void Role_Ctor_Should_Be_Correct()
    {
        const string roleId = "test";
        const string name = "Test123";

        var role = new Role(roleId, name);

        role.RoleId.ShouldBe(roleId);
        role.Name.ShouldBe(name);
        role.RoleScopes.ShouldNotBeNull();
        role.RoleModules.ShouldNotBeNull();
    }
    
    [Fact]
    public void Role_Ctor_Should_Be_Correct_2()
    {
        const string name = "Staff Warehouse";

        var role = new Role(name);

        role.RoleId.ShouldBe("staffWarehouse");
        role.Name.ShouldBe(name);
        role.RoleScopes.ShouldNotBeNull();
        role.RoleModules.ShouldNotBeNull();
    }
}