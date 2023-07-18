using System.Text.RegularExpressions;
using OrderManagementApi.Shared.Abstractions.Models;

namespace OrderManagementApi.Domain.Extensions;

public static partial class RoleExtensions
{
    public const string SuperAdministrator = "Super Administrator";
    public const string SuperAdministratorId = "superadministrator";
    public const string Administrator = "Administrator";
    public const string AdministratorId = "administrator";

    public static readonly List<DropdownKeyValue> AvailableRoles = new()
    {
        new DropdownKeyValue(AdministratorId, Administrator)
    };

    public static string SetToRoleId(this string s)
    {
        return RegexSpaces().Replace(s, "");
    }

    [GeneratedRegex("\\s+")]
    private static partial Regex RegexSpaces();
}