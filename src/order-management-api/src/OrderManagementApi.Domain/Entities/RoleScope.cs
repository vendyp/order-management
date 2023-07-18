using OrderManagementApi.Shared.Abstractions.Entities;

namespace OrderManagementApi.Domain.Entities;

public sealed class RoleScope : BaseEntity
{
    private RoleScope()
    {
        RoleScopeId = Guid.NewGuid();
        RoleId = string.Empty;
        Name = string.Empty;
    }

    public RoleScope(string roleId, string name) : this()
    {
        RoleId = roleId;
        Name = name;
    }

    public Guid RoleScopeId { get; set; }
    public string RoleId { get; set; }
    public string Name { get; set; }
}