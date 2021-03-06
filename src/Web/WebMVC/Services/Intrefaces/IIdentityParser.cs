using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace WebMVC.Services.Intrefaces
{
    public interface IIdentityParser<T>
    {
        T Parse(IPrincipal principal);
    }
}
