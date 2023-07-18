using OrderManagementApi.Domain.Extensions;
using OrderManagementApi.Shared.Abstractions.Entities;
using OrderManagementApi.Shared.Abstractions.Helpers;

namespace OrderManagementApi.Domain.Entities;

public sealed class Role : BaseEntity
{
    private Role()
    {
        RoleId = string.Empty;
        Name = string.Empty;
        RoleScopes = new HashSet<RoleScope>();
        RoleModules = new HashSet<RoleModule>();
    }

    public Role(string name) : this()
    {
        RoleId = name.SetToRoleId().ToCamelCase();
        Name = name;
    }

    public Role(string roleId, string name) : this()
    {
        RoleId = roleId;
        Name = name;
    }

    public string RoleId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public ICollection<RoleScope> RoleScopes { get; }
    public ICollection<RoleModule> RoleModules { get; }
}