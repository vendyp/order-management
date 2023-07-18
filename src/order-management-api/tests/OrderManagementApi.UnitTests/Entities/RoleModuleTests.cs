using OrderManagementApi.Domain.Entities;
using Shouldly;

namespace OrderManagementApi.UnitTests.Entities;

public class RoleModuleTests
{
    [Fact]
    public void RoleModule_Ctor_ShouldBe_Correct()
    {
        const string roleId = "test";
        const string name = "test123";

        var roleModule = new RoleModule(roleId, name);

        roleModule.RoleId.ShouldBe(roleId);
        roleModule.Name.ShouldBe(name);
        roleModule.RoleModuleId.ShouldNotBe(default);
    }
}