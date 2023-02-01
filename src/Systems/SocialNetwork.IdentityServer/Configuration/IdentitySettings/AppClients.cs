using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using SocialNetwork.Constants.Security;

namespace SocialNetwork.IdentityServer.Configuration.IdentitySettings;

public static class AppClients
{
    public static IEnumerable<Client> Clients = new[]
    {
        new Client
        {
            ClientId = "swagger",
            ClientSecrets = {new Secret("secret".Sha256())},
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AllowedScopes =
            {
                AppScopes.NetworkRead
            }
        },
        new Client
        {
            ClientId = "frontend",
            ClientSecrets = {new Secret("secret".Sha256())},
            AllowAccessTokensViaBrowser = true,
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

            AllowOfflineAccess = true,
            AccessTokenType = AccessTokenType.Jwt,

            AccessTokenLifetime = 3600 * 12, // 12 hours

            RefreshTokenUsage = TokenUsage.OneTimeOnly,
            RefreshTokenExpiration = TokenExpiration.Sliding,
            AbsoluteRefreshTokenLifetime = 2592000, // 30 days
            SlidingRefreshTokenLifetime = 1296000, // 15 days

            AllowedScopes = new List<string>
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                AppScopes.NetworkRead,
                AppScopes.NetworkWrite
            }
        }
    };
}