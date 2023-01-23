using Duende.IdentityServer.Models;
using IdentityModel;
using SocialNetwork.Common.Security;

namespace SocialNetwork.IdentityServer.Configuration.IdentitySettings;

public static class AppIdentityResources
{
    public static IEnumerable<IdentityResource> Resources = new IdentityResource[]
    {
        new IdentityResources.OpenId()
        {
            UserClaims =
            {
                JwtClaimTypes.NickName,
                JwtClaimTypes.Email,
                AppClaimTypes.Role
            }
        },
        new IdentityResources.Profile()
        {
            UserClaims =
            {
                JwtClaimTypes.NickName,
                JwtClaimTypes.Email,
                AppClaimTypes.Role
            }
        }
    };
}