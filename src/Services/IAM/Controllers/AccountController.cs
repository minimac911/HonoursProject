using IAM.Models;
using IAM.Models.AccountViewModels;
using IAM.Services;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IAM.Controllers.Account
{
    public class AccountController : Controller
    {
        // Class variables
        private readonly ILoginService<ApplicationUser> _loginService;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        // init class vars
        public AccountController(
            ILoginService<ApplicationUser> loginService,
            UserManager<ApplicationUser> userManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            ILogger<AccountController> logger,
            IConfiguration configuration)
        {
            _loginService = loginService;
            _userManager = userManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _logger = logger;
            _configuration = configuration;
        }

        // Show login page
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
                throw new NotImplementedException("External login is not implemented!");
            }
            // build the login view model
            var vm = BuildLoginViewModelAsync(returnUrl, context);

            ViewData["ReturnUrl"] = returnUrl;
            // return the login view
            return View(vm);
        }

        // Login the user with username and password
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // check the model state
            if (ModelState.IsValid)
            {
                // get user using username
                var foundUser = await _loginService.FindUserByUsername(model.Username);

                // check password with hashed password
                if (await _loginService.validateUserCrendtials(foundUser, model.Password))
                {
                    // get token lifetime
                    var tokenLife = _configuration.GetValue("TokenLifeTimeInMinutes", 60 * 2);
                    // auth props 
                    var properties = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(tokenLife),
                        AllowRefresh = true,
                        RedirectUri = model.ReturnUrl
                    };
                    // signin the user
                    await _loginService.SingInAsync(foundUser, properties);
                    // check return url
                    if (_interaction.IsValidReturnUrl(model.ReturnUrl))
                    {
                        // redirect to return url
                        return Redirect(model.ReturnUrl);
                    }

                    // redirect to home 
                    return Redirect("~/");
                }
                // username or password is wrong
                ModelState.AddModelError("", "Username or password is incorrect");
            }

            //an error has occured with the form
            // create a viewmodel to send back to the login page
            var vm = await BuildLoginViewModelAsync(model);

            ViewData["ReturnUrl"] = model.ReturnUrl;

            return View(vm);
        }

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
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            // create view model
            var vm = BuildLoginViewModelAsync(model.ReturnUrl, context);
            // only remeber the username and dont pass through the password
            vm.Username = model.Username;

            return vm;
        }

        // view logout screen
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // if user is already logged out then just show logged out page
            if (User.Identity.IsAuthenticated == false)
            {
                return await Logout(new LogoutViewModel() { LogoutId = logoutId });
            }

            // show logout screen
            var vm = new LogoutViewModel()
            {
                LogoutId = logoutId
            };

            return View(vm);
        }

        // Post logout form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutViewModel model)
        {
            // get identity provider
            var idp = User?.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
            // if the identity provider is valid
            if(idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
            {
                if(model.LogoutId != null)
                {
                    // if there is no logout context
                    // then create one
                    model.LogoutId = await _interaction.CreateLogoutContextAsync();
                }

                // 
                var redirectUrl = $"/Account/Logout?logoutId={model.LogoutId}";

                try
                {
                    // signout
                    await HttpContext.SignOutAsync(idp, new AuthenticationProperties
                    {
                        RedirectUri = redirectUrl
                    });
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Error when logging out: ", ex.Message);
                }
            }

            // delete authentication cookie 
            await HttpContext.SignOutAsync();

            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            // make user be anonomys 
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // get logout context using logout id
            var logoutContext = await _interaction.GetLogoutContextAsync(model.LogoutId);

            // redirect to post logout url
            return Redirect(logoutContext?.PostLogoutRedirectUri);
        }

        // View register page
        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // Post register form
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            // set view data ReturnUrl
            ViewData["ReturnUrl"] = returnUrl;
            // if model is vaild
            if (ModelState.IsValid)
            {
                // create new user
                var createUser = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Username,
                };
                
                // add user to database
                var result = await _userManager.CreateAsync(createUser, model.Password);
                
                // check for errors when creating the user
                if(result.Errors.Count() > 0)
                {
                    // add errors to models states
                    AddErrorsToModelState(result);
                    return View(model);
                }
                if (returnUrl != null)
                {
                    if (HttpContext.User.Identity.IsAuthenticated)
                    {
                        return Redirect(returnUrl);
                    }
                    else if (ModelState.IsValid)
                    {
                        return RedirectToAction("login", "account", new { returnUrl = returnUrl });
                    }
                    else
                    {
                        return View(model);
                    }
                }
            }

            return RedirectToAction("index", "home");
        }

        private void AddErrorsToModelState(IdentityResult result)
        {
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
