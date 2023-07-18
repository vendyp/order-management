namespace OrderManagementApi.WebApi.Dto;

public sealed class RoleDetailDto : RoleDto
{
    public RoleDetailDto(string roleId, string name, string? description) : base(roleId, name, description)
    {
        Scopes = new List<ScopeDto>();
    }
    
    public List<ScopeDto> Scopes { get; }
}