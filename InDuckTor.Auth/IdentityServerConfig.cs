using IdentityServer4;
using IdentityServer4.Models;

namespace InDuckTor.Auth
{
    public class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client {
                    RequireConsent = false,
                    ClientId = "angular_spa",
                    ClientName = "Angular SPA",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = { "openid", "profile", "email" },
                    RedirectUris = {"http://localhost:4200/auth-callback"},
                    PostLogoutRedirectUris = {"http://localhost:4200/home"},
                    AllowedCorsOrigins = {"http://localhost:4200"},
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 360000
                },
                 new Client
                {
                    ClientId = "inductor_mobile_client",
                    ClientName = "Android app client",
                    RequireClientSecret = false,
                    RequirePkce = false,
                    RequireConsent = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris =
                        { "com.ithirteeng.secondpatternsclientproject.app://yo" },
                    AllowedScopes = { "openid", "profile", "email" },
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 360000
                }
            };
        }
    }
}
