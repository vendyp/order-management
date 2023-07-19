using OrderManagementApi.Domain.Entities;

namespace OrderManagementApi.WebApi.Dto;

public class UserDto
{
    public UserDto()
    {
        Scopes = new List<string>();
    }

    public UserDto(User user) : this()
    {
        UserId = user.UserId;
        Username = user.Username;
        FullName = user.FullName;
        LastPasswordChangeAt = user.LastPasswordChangeAt;
        Email = user.Email;
        CreatedAt = user.CreatedAt;
        CreatedByName = user.CreatedByName;
        LastUpdatedAt = user.LastUpdatedAt;
        LastUpdatedByName = user.LastUpdatedByName;
    }

    public Guid? UserId { get; set; }
    public string? Username { get; set; }
    public string? FullName { get; set; }
    public DateTime? LastPasswordChangeAt { get; set; }
    public string? Email { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? CreatedByName { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public string? LastUpdatedByName { get; set; }

    public List<string> Scopes { get; set; }
}