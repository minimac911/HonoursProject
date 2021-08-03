// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace IAM
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };


        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("catalog"),
                new ApiScope("cart")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("catalog")
                {
                    Scopes = new List<string>{ "catalog" },
                    ApiSecrets = new List<Secret>{ new Secret("secret".Sha256()) }
                },
                new ApiResource("cart")
                {
                    Scopes = new List<string>{ "cart" },
                    ApiSecrets = new List<Secret>{ new Secret("secret".Sha256()) }
                }
            };

        public static IEnumerable<Client> Clients(IConfiguration configuration)
        {
            var urlMVC = configuration["MvcClient"];
            return new List<Client>
            {
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "Main MVC",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    ClientUri = $"{urlMVC}",
                    AllowAccessTokensViaBrowser = false,
                    RequireConsent = false,
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    // where to redirect to after login
                    RedirectUris = { $"{urlMVC}/signin-oidc" },
                    // where to redirect to after logout
                    PostLogoutRedirectUris = { $"{urlMVC}/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "catalog",
                        "cart"
                    },
                    AccessTokenLifetime = 60*60*2, // 2 hours
                    IdentityTokenLifetime= 60*60*2 // 2 hours

                }
            };
        }
    }
}