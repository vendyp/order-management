using System.Security.Claims;
using OrderManagementApi.Core.Modules;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Domain.Extensions;
using OrderManagementApi.WebApi.Scopes;

namespace OrderManagementApi.WebApi.Endpoints.Identity.Helpers;

public static class ClaimsGenerator
{
    public static Dictionary<string, IEnumerable<string>> Generate(User user)
    {
        var claims = new Dictionary<string, IEnumerable<string>>
        {
            ["xid"] = new[] { user.UserId.ToString() },
            ["usr"] = new[] { user.Username },
        };

        foreach (var userRole in user.UserRoles)
        {
            claims.Add(ClaimTypes.Role, new[] { userRole.RoleId });

            if (userRole.Role != null && userRole.Role.RoleScopes.Any())
            {
                claims.Add("scopes", userRole.Role!.RoleScopes.Select(e => e.Name));
            }

            if (userRole.Role != null && userRole.Role.RoleModules.Any())
            {
                claims.Add("modules", userRole.Role!.RoleModules.Select(e => e.Name));
            }
        }

        if (!string.IsNullOrWhiteSpace(user.Email))
            claims.Add(ClaimTypes.Email, new[] { user.Email });

        //if normal
        if (claims[ClaimTypes.Role].Any(e => e != RoleExtensions.SuperAdministratorId))
            return claims;

        claims.Remove("scopes");

        claims.Remove("modules");

        claims.Add("scopes", ScopeManager.Instance.GetAllScopes());

        claims.Add("modules", new ModuleManager().GetAllModuleName());

        return claims;
    }
}