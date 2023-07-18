using OrderManagementApi.Shared.Abstractions.Entities;

namespace OrderManagementApi.Domain.Entities;

public sealed class UserToken : BaseEntity, IEntity
{
    public UserToken()
    {
        UserTokenId = Guid.NewGuid();
        RefreshToken = string.Empty;
        IsUsed = false;
    }

    public UserToken(Guid userId, string refreshToken, DateTime expiryAt) : this()
    {
        UserId = userId;
        RefreshToken = refreshToken;
        ExpiryAt = expiryAt;
    }

    public Guid UserTokenId { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string RefreshToken { get; set; }

    /// <summary>
    /// Expiration of refresh token key
    /// </summary>
    public DateTime ExpiryAt { get; set; }

    /// <summary>
    /// Flag that will use to identify refresh token key is already used
    /// </summary>
    public bool IsUsed { get; set; }

    /// <summary>
    /// When that refresh token key successfully used
    /// </summary>
    public DateTime? UsedAt { get; set; }

    /// <summary>
    /// Update IsUsed and UsedAt properties.
    /// </summary>
    /// <param name="dt">DateTime</param>
    public void UseUserToken(DateTime dt)
    {
        IsUsed = true;
        UsedAt = dt;
    }
}