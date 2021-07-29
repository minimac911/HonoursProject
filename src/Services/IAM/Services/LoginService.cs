using IAM.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAM.Services
{
    public class LoginService : ILoginService<ApplicationUser>
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public LoginService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ApplicationUser> FindUserByUsername(string username)
        {
            // get user using username
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<bool> validateUserCrendtials(ApplicationUser user, string password)
        {
            // check password
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public Task SignIn(ApplicationUser user)
        {
            // sign in the user and make it persistent
            return _signInManager.SignInAsync(user, true);
        }

        public Task SingInAsync(ApplicationUser user, AuthenticationProperties properties, string authMethod = null)
        {
            // sing in the user using auth properties and auth method
            return _signInManager.SignInAsync(user, properties, authMethod);
        }
    }
}
