namespace OrderManagementApi.WebApi.Endpoints.UserManagement;

public class CreateUserRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Fullname { get; set; }
    public string? Role { get; set; }
    public string? EmailAddress { get; set; }
}