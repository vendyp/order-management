using OrderManagementApi.Domain.Entities;

namespace OrderManagementApi.WebApi.Dto;

public class LoginDto
{
    public LoginDto(User user)
    {
        UserId = user.UserId;
    }

    public Guid UserId { get; }
    public long Expiry { get; init; }
    public string AccessToken { get; init; } = null!;
    public string RefreshToken { get; init; } = null!;
}