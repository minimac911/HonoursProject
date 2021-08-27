using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Services
{
    public interface IIdentityService
    {
        string GetUserId();
    }
}
