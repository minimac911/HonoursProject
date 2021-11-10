using IAM.Data;
using IAM.Models;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IAM.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TenantContext _tenantContext;

        public ProfileService(
            UserManager<ApplicationUser> userManager, 
            TenantContext tenantContext)
        {
            _userManager = userManager;
            _tenantContext = tenantContext;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            // get the subject
            var sub = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            // get the subject id
            var subId = sub.Claims.Where(x => x.Type == JwtClaimTypes.Subject).FirstOrDefault().Value;
            // get the user 
            var user = await _userManager.FindByIdAsync(subId);

            if (user == null) throw new AggregateException("Inavlid subject id");

            var claims = GetUserClaims(user);
            // set the issued claims to the geneterated ones
            context.IssuedClaims = claims.ToList();
            // get role claims and add them to issued claims
            var roleClaims = context.Subject.FindAll(JwtClaimTypes.Role);
            context.IssuedClaims.AddRange(roleClaims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            // set it so that the user is always active
            context.IsActive = true;
        }

        private IEnumerable<Claim> GetUserClaims(ApplicationUser user)
        {
            // get the tenant id
            var tenant = _tenantContext.Tenant.FirstOrDefault(x => x.Id == user.TenantId);
            return new List<Claim>()
            {
                // set the subject to user id
                new Claim(JwtClaimTypes.Subject, user.Id),
                // set the username
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName),
                // set the unique name to the user name
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                // set the tenant Id and tenant name
                new Claim("tenant", user.TenantId.ToString()),
                new Claim("tenant_name", tenant.Name)
            };
        }
    }
}
