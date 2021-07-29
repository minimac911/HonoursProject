using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace WebMVC.Infrastructure.Http
{
    public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler 
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpClientAuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // get authorization header
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            // Add authorization header
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                request.Headers.Add("Authorization", new List<string>() { authorizationHeader });
            }

            // Get the token
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            // If there is a token then add it to the headers
            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            // send async with authorization
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
