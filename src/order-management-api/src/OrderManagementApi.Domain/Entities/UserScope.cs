using OrderManagementApi.Shared.Abstractions.Entities;

namespace OrderManagementApi.Domain.Entities;

public class UserScope : BaseEntity
{
    public Guid UserScopeId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string ScopeId { get; set; } = null!;
}