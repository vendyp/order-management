namespace OrderManagementApi.WebApi.Endpoints.Identity;

public class SignInRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}