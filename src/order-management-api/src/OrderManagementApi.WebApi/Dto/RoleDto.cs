namespace OrderManagementApi.WebApi.Dto;

public class RoleDto
{
    public RoleDto(string roleId, string name, string? description)
    {
        RoleId = roleId;
        Name = name;
        Description = description;
    }
    
    public string RoleId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}