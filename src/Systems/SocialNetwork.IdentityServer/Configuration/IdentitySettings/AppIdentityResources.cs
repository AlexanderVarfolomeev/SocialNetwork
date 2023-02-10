using Duende.IdentityServer.Models;
using IdentityModel;

namespace SocialNetwork.IdentityServer.Configuration.IdentitySettings;

public static class AppIdentityResources
{
    public static IEnumerable<IdentityResource> Resources = new IdentityResource[]
    {
        new IdentityResources.OpenId
        {
            UserClaims =
            {
                JwtClaimTypes.NickName,
                JwtClaimTypes.Email,
            }
        },
        new IdentityResources.Profile
        {
            UserClaims =
            {
                JwtClaimTypes.NickName,
                JwtClaimTypes.Email,
            }
        }
    };
}