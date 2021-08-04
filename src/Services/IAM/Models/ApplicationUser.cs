using Microsoft.AspNetCore.Identity;
using System;

namespace IAM.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Guid TenantId { get; set; }
    }
}