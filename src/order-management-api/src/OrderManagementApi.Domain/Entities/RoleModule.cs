using OrderManagementApi.Shared.Abstractions.Entities;

namespace OrderManagementApi.Domain.Entities;

public sealed class RoleModule : BaseEntity
{
    private RoleModule()
    {
        RoleModuleId = Guid.NewGuid();
        RoleId = string.Empty;
        Name = string.Empty;
    }

    public RoleModule(string roleId, string name) : this()
    {
        RoleId = roleId;
        Name = name;
    }

    public Guid RoleModuleId { get; set; }
    public string RoleId { get; set; }
    public string Name { get; set; }
}