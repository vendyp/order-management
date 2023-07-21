using System.Security.Claims;
using OrderManagementApi.Domain.Entities;

namespace OrderManagementApi.WebApi.Shared.Helpers;

public static class ClaimsGenerator
{
    public static Dictionary<string, IEnumerable<string>> Generate(User user)
    {
        var claims = new Dictionary<string, IEnumerable<string>>
        {
            ["xid"] = new[] { user.UserId.ToString() },
            ["usr"] = new[] { user.Username },
            ["scopes"] = new[] { nameof(UserScope).ToLower() }
        };

        if (!string.IsNullOrWhiteSpace(user.Email))
            claims.Add(ClaimTypes.Email, new[] { user.Email });

        if (!user.UserScopes.Any()) return claims;

        var list = new List<string> { nameof(UserScope).ToLower() };
        list.AddRange(user.UserScopes.Select(e => e.ScopeId));
        claims["scopes"] = list.ToArray();

        return claims;
    }
}