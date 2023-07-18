namespace OrderManagementApi.WebApi.Endpoints.RoleManagement;

public class EditRoleRequestPayload
{
    public EditRoleRequestPayload()
    {
        Scopes = new List<string>();
        Modules = new List<string>();
    }

    public string? Description { get; set; }
    public List<string> Scopes { get; set; }
    public List<string> Modules { get; set; }
}