using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace IAM
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog"),
                new ApiScope("cart"),
                new ApiScope("orders"),
            };

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            return new List<Client> {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "Main Mvc Clinet",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    ClientUri = $"{configuration.GetValue<string>("urls__mvc")}",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = new List<string>{ $"{configuration.GetValue<string>("urls__mvc")}/signin-oidc" },
                    PostLogoutRedirectUris = new List<string>{  $"{configuration.GetValue<string>("urls__mvc")}/signout-callback-oidc" },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "catalog",
                        "cart",
                        "orders"
                    },
                    AccessTokenLifetime = 60*60*2, // 2 hours
                    IdentityTokenLifetime= 60*60*2 // 2 hours
                }
            };
        }
    }
}