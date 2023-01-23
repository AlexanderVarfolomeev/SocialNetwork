using Duende.IdentityServer.Models;
using SocialNetwork.Common.Security;

namespace SocialNetwork.IdentityServer.Configuration.IdentitySettings;

public static class AppApiResources
{
    public static IEnumerable<ApiResource> Resources => new List<ApiResource>
    {
        new ApiResource(ApiResources.SocialNetwork, "Social network API")
    };
}