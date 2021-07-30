using IdentityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using WebMVC.Models;
using WebMVC.Services.Intrefaces;

namespace WebMVC.Services
{
    public class IdentityParser : IIdentityParser<ApplicationUser>
    {
        public ApplicationUser Parse(IPrincipal principal)
        {
            // test to see if the principal is a claims principal
            if (principal is ClaimsPrincipal claims)
            {
                return new ApplicationUser()
                {
                    Id = claims.FindFirst(x => x.Type == JwtClaimTypes.Subject)?.Value ?? "",
                    UserName = claims.FindFirst(x => x.Type == JwtClaimTypes.PreferredUserName)?.Value ?? "",
                };

            }
            throw new ArgumentException(message: "The principla must be a ClaimsPrincipal", paramName: nameof(principal));
        }
    }
}
