using OrderManagementApi.Shared.Abstractions.Entities;

namespace OrderManagementApi.Domain.Entities;

public sealed class UserRole : BaseEntity, IEntity
{
    public UserRole()
    {
        RoleId = string.Empty;
    }

    public UserRole(Guid userId, string roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }

    public Guid UserId { get; set; }
    public string RoleId { get; set; }

    public Role? Role { get; set; }
    public User? User { get; set; }
}