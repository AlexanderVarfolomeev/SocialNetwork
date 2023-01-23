using Duende.IdentityServer.Models;
using IdentityModel;
using SocialNetwork.Common.Security;

namespace SocialNetwork.IdentityServer.Configuration.IdentitySettings;

public class AppApiScopes
{
    public static IEnumerable<ApiScope> Scopes = new[]
    {
        new ApiScope(AppScopes.NetworkRead, "Access to TwitterApi - read data.")
        {
            UserClaims =
            {
                JwtClaimTypes.NickName,
                JwtClaimTypes.Email,
                AppClaimTypes.Role
            }
        },
        new ApiScope(AppScopes.NetworkWrite, "Access to TwitterApi - write data.")
        {
            UserClaims =
            {
                JwtClaimTypes.NickName,
                JwtClaimTypes.Email,
                AppClaimTypes.Role
            }
        },
    };
}