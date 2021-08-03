using IAM.Models;
using IAM.Services;
using IAM.ViewModels;
using IAM.ViewModels.Account;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServerHost.Quickstart.UI
{
    public class AccountController : Controller
    {
        private readonly ILoginService<ApplicationUser> _loginService;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;

        public AccountController(
            ILoginService<ApplicationUser> loginService,
            IIdentityServerInteractionService interaction,
            UserManager<ApplicationUser> userManager,
            ILogger<AccountController> logger,
            IConfiguration configuration)
        {
            _loginService = loginService;
            _interaction = interaction;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

            if(context?.IdP != null)
            {
                throw new Exception("External login is not available");
            }

            // Build The login view
            var vm = BuildLoginViewModelAsync(returnUrl, context);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // check if the model state is 
            if (ModelState.IsValid)
            {
                // get the user
                var foundUser = await _loginService.FindByUsername(model.Username);
                // check if the password is valid for the username
                var isPasswordValid = await _loginService.ValidateUserCredentials(foundUser, model.Password);
                if (isPasswordValid)
                {
                    // get the lifetime of the token
                    var tokenLifeTime = _configuration.GetValue("TokenLifetimeInMinutes", 60);

                    var properties = new AuthenticationProperties()
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(tokenLifeTime),
                        AllowRefresh = true, 
                        RedirectUri = model.ReturnUrl
                    };

                    // if the remeber me was checked
                    if (model.RememberLogin)
                    {
                        var permaTokenLifeTime = _configuration.GetValue("PermamentTokenLifetimeInDays", 365);
                        
                        properties.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(permaTokenLifeTime);
                        properties.IsPersistent = true;
                    }

                    // sign in the user with the auth properties
                    await _loginService.SignInAsync(foundUser, properties);

                    if (_interaction.IsValidReturnUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return Redirect("~/");
                }

                ModelState.AddModelError("", "Invalid Credentials");
            }

            // something went wrong, show form with error
            var vm = await BuildLoginViewModelAsync(model);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // user not authenticated
            if (User.Identity.IsAuthenticated == false)
            {
                return await Logout(new LogoutViewModel { LogoutId = logoutId });
            }

            var vm = new LogoutViewModel
            {
                LogoutId = logoutId
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutViewModel model)
        {
            // find the identity provider
            var idp = User?.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

            if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
            {
                _logger.LogError("ERROR: External identity provrider has been used");
            }

            // delete cookie
            await HttpContext.SignOutAsync();

            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            // set user to be a blank user 
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // get the logout conext
            var logout = await _interaction.GetLogoutContextAsync(model.LogoutId);

            // redirect to post logout redirect
            return Redirect(logout?.PostLogoutRedirectUri);
        }


        /********************************/
        /*Helpers for Account Controller*/
        /********************************/
        private LoginViewModel BuildLoginViewModelAsync(string returnUrl, AuthorizationRequest context)
        {
            return new LoginViewModel
            {
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginViewModel model)
        {
            // get context
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            // build login view model
            var vm = BuildLoginViewModelAsync(model.ReturnUrl, context);
            // set the username and the remeber me choice
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;

            return vm;
        }
    }
}
