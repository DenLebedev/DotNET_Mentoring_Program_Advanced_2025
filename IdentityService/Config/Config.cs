using Duende.IdentityServer.Models;

namespace IdentityService.Config
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("roles", "User role(s)", new[] { "role" })
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_api", "Access to Catalog API"),
                new ApiScope("carting_api", "Access to Carting API"),
                new ApiScope("offline_access", "Access to Refresh Token")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("catalog_api_resource", "Catalog API")
                {
                    Scopes = { "catalog_api" },
                    UserClaims = { "role", "permission" }
                },
                new ApiResource("carting_api_resource", "Carting API")
                {
                    Scopes = { "carting_api" },
                    UserClaims = { "role", "permission" }
                }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "swagger-ui",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("swagger-secret".Sha256()) },
                    AllowedScopes = {
                        "openid", "profile", "roles", "catalog_api", "carting_api", "offline_access"
                    },
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 900,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    AbsoluteRefreshTokenLifetime = 2592000
                }
            };
    }
}
