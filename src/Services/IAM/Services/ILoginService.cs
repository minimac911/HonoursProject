using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAM.Services
{
    public interface ILoginService<T>
    {
        Task<bool> validateUserCrendtials(T user, string password);

        Task<T> FindUserByUsername(string username);

        Task SignIn(T user);

        Task SingInAsync(T user, AuthenticationProperties properties, string authMethod = null);
    }
}
