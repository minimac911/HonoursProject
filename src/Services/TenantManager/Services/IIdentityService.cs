using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenantManager.Services
{
    interface IIdentityService
    {
        string GetUserId();
    }
}
