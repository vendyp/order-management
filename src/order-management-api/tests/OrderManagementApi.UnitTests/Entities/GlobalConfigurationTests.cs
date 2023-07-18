using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Entities;
using Shouldly;

namespace OrderManagementApi.UnitTests.Entities;

public class GlobalConfigurationTests
{
    [Fact]
    public void Ctor_GlobalConfiguration_ShouldBe_Ok()
    {
        _ = new Option();
    }

    [Fact]
    public void Ctor_User_GetType_Must_Satisfied_Design()
    {
        var type = typeof(Option);
        type.IsSealed.ShouldBeTrue();
        type.BaseType.ShouldNotBeNull();
        type.BaseType.ShouldBe(typeof(BaseEntity));
        typeof(IEntity).IsAssignableFrom(type).ShouldBeTrue();
    }
}