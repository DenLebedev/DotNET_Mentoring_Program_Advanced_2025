using Duende.IdentityServer.Models;

namespace IdentityService.Config
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),  // Required for OpenID Connect
                new IdentityResources.Profile(), // Optional: includes name, email, etc.
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_api", "Access to Catalog API"),
                new ApiScope("carting_api", "Access to Carting API"),
                new ApiScope("offline_access", "Access to Refresh Token") // Built-in by spec
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("catalog_api_resource", "Catalog API")
                {
                    Scopes = { "catalog_api" }
                },
                new ApiResource("carting_api_resource", "Carting API")
                {
                    Scopes = { "carting_api" }
                }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "swagger-ui",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword, // For Swagger testing
                    ClientSecrets = { new Secret("swagger-secret".Sha256()) },
                    AllowedScopes = {
                        "openid", "profile", "catalog_api", "carting_api", "offline_access"
                    },
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 900, // 15 mins
                    RefreshTokenUsage = TokenUsage.ReUse, // Don't create a new one every time
                    AbsoluteRefreshTokenLifetime = 2592000 // 30 days
                }
            };
    }
}
