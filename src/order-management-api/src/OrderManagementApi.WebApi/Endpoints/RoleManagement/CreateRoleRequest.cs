namespace OrderManagementApi.WebApi.Endpoints.RoleManagement;

public class CreateRoleRequest
{
    public CreateRoleRequest()
    {
        Scopes = new List<string>();
        Modules = new List<string>();
    }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public List<string> Scopes { get; set; }
    
    public List<string> Modules { get; set; }
}