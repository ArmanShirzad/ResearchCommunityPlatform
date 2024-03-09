using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ResearchCommunityPlatform.Models;

namespace ResearchCommunityPlatform.Controllers
{
    [Controller]
    public class AccountController : Controller
    {

        public IActionResult Login(string returnUrl = "/dashboard")
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        // External login action
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "/dashboard", string remoteError = null)
        {
            // Handle external login callback logic
            // On success, redirect to returnUrl
            return Ok();
        }
        public async Task<IActionResult> SocialMediaLogin() 
        {
            return View();
        }

       // Dashboard action
       [Authorize]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
