using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAM.Services
{
    public interface ILoginService <T>
    {
        public Task<bool> ValidateUserCredentials(T user, string password);

        public Task<T> FindByUsername(string username);

        public Task SignIn(T user);

        public Task SignInAsync(T user, AuthenticationProperties properties);
    }
}
