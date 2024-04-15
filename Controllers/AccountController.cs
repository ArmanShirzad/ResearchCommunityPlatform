using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using ResearchCommunityPlatform.Models;

using System.Security.Claims;

namespace ResearchCommunityPlatform.Controllers
{
    [Controller]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userInManager;


        public AccountController(SignInManager<User> signInManager, UserManager<User> userInManager)
        {
            _signInManager = signInManager;
            _userInManager = userInManager;
        }

        public IActionResult Login(string returnUrl = "/dashboard")
        {   
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        // External login action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            //var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                // Handle the error scenario here, possibly redirecting to an error page
                return RedirectToAction(nameof(Login), new { ErrorMessage = remoteError });
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                // Handle the error scenario when external login info is not available
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
                if (result.Succeeded)
                {
                // If we have a valid returnUrl that is a local URL, use it; otherwise, redirect to "/dashboard"
                if (Url.IsLocalUrl(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    // If returnUrl is null, empty, or not local, redirect to the Home controller's Index action.
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                // User does not exist in your system, create a new user account
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);

                // Pass additional info to CreateUser method
                var newUser = await CreateUser(email, name, info.LoginProvider, info.ProviderKey);

                // Check if user creation was successful
                if (newUser != null)
                {
                    // Sign in the newly created user
                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    // After signing in, redirect to returnUrl if it's local or "/dashboard" if it's null/invalid
                    return LocalRedirect(returnUrl ?? "/dashboard");
                }
                else
                {
                    // Handle error in user creation
                    await Console.Out.WriteLineAsync("user not created");
                    return NoContent();
                }
            }
        }
        public async Task<IActionResult> SocialMediaLogin() 
        {
            ViewData["HideSearch"] = true;
            ViewData["HideNavItmes"] = true;

            return View();
        }


        private async Task<User> CreateUser(string email, string name, string loginProvider, string providerKey)
        {
            var user = new User { UserName = email, Email = email};
            var result = await _userInManager.CreateAsync(user);
            if (result.Succeeded)
            {
                // Create the link between the new user and the external login
                var loginInfo = new UserLoginInfo(loginProvider, providerKey, name);
                var addLoginResult = await _userInManager.AddLoginAsync(user, loginInfo);

                if (addLoginResult.Succeeded)
                {
                    return user;
                }
                // Optionally handle the situation where the login couldn't be added
            }

            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
