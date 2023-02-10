using SocialNetwork.Common.Enum;

namespace SocialNetwork.Common.Extensions;

public static class PermissionsExtensions
{
    public static string GetName(this Permissions permission) =>
        permission switch
        {
            Permissions.User => "User",
            Permissions.Admin => "Admin",
            Permissions.GodAdmin => "GodAdmin",
            _ => "None"
        };
}