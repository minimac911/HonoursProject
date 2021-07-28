using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebMVC.Extensions
{
    public static class HttpExtensions
    {
        public static async Task HandleToken(this HttpClient client, string authority, string clientId, string secret, string apiName)
        {
            var accessToken = await client.GetRefreshTokenAsync(authority, clientId, secret, apiName);
            client.SetBearerToken(accessToken);
        }

        private static async Task<string> GetRefreshTokenAsync(this HttpClient client, string authority, string clientId, string secret, string apiName)
        {
            var disco = await client.GetDiscoveryDocumentAsync(authority);
            if (disco.IsError) throw new Exception(disco.Error);

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = clientId,
                ClientSecret = secret,
                Scope = apiName
            });

            if (!tokenResponse.IsError) return tokenResponse.AccessToken;
            return null;
        }
    }
}
