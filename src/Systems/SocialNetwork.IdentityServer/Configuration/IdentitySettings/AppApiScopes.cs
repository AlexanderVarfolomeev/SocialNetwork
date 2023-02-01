using Duende.IdentityServer.Models;
using IdentityModel;
using SocialNetwork.Constants.Security;

namespace SocialNetwork.IdentityServer.Configuration.IdentitySettings;

public class AppApiScopes
{
    public static IEnumerable<ApiScope> Scopes = new[]
    {
        new ApiScope(AppScopes.NetworkRead, "Access to read data.")
        {
            UserClaims =
            {
                JwtClaimTypes.NickName,
                JwtClaimTypes.Email,
            }
        },
        new ApiScope(AppScopes.NetworkWrite, "Access to write data.")
        {
            UserClaims =
            {
                JwtClaimTypes.NickName,
                JwtClaimTypes.Email,
            }
        },
    };
}