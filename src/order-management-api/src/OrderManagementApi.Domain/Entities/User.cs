﻿using OrderManagementApi.Shared.Abstractions.Entities;

namespace OrderManagementApi.Domain.Entities;

public sealed class User : BaseEntity, IEntity
{
    public User()
    {
        UserId = Guid.NewGuid();
        Username = string.Empty;
        UserTokens = new HashSet<UserToken>();
        UserScopes = new HashSet<UserScope>();
    }

    public User(string username) : this()
    {
        Username = username;
        NormalizedUsername = Username.ToUpper();
    }

    public Guid UserId { get; set; }
    public string Username { get; set; }
    private string? _username;

    public string NormalizedUsername
    {
        get => _username?.ToUpper() ?? string.Empty;
        set => _username = value;
    }

    public string? Salt { get; set; }
    public string? Password { get; set; }
    public DateTime? LastPasswordChangeAt { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }

    public ICollection<UserToken> UserTokens { get; set; }
    public ICollection<UserScope> UserScopes { get; set; }

    public void UpdatePassword(string salt, string password)
    {
        if (string.IsNullOrWhiteSpace(salt))
            throw new ArgumentNullException(nameof(salt));
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException(nameof(password));

        Salt = salt;
        Password = password;
    }
}