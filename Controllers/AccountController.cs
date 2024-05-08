using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using NuGet.Protocol.Plugins;

using ResearchCommunityPlatform.Models;
using ResearchCommunityPlatform.Services.UserSevice;

using System.Security.Claims;

namespace ResearchCommunityPlatform.Controllers
{
    [Controller]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userInManager;
        private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;




        public AccountController(SignInManager<User> signInManager, UserManager<User> userInManager, IUserService userService, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userInManager = userInManager;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var state = await _userService.LoginUserAsync(model.UserName, model.Password);
                if (state)
                {
                    ViewBag.state = $"welcom!, {model.UserName}";
                    return View("socialmedialogin", model);

                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
            return View(model);
        }
        // External login action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

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
                    return LocalRedirect(returnUrl ?? "/");
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
            var user = new User { UserName = email, Email = email };
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
        [HttpGet]
        public async Task<IActionResult> GoogleLoginCallback(string returnUrl = "/")
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return BadRequest("Error while authenticating with Google."); // Or handle this scenario appropriately

            // Create claims and sign in the user with the local cookie scheme
            var claimsPrincipal = authenticateResult.Principal;
            var claimsIdentity = new ClaimsIdentity(claimsPrincipal.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return LocalRedirect(returnUrl);
        }

        #region Register

        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp([FromForm] RegisterViewModel model)
        {
            if (_userService.UsernameExists(model.UserName))
            {
                ModelState.AddModelError("UserName", "username already exists");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(model);
                if (result.Success)
                {
                    TempData["UserEmail"] = model.Email; // Store email to TempData

                    // Inform the user to check their email for a confirmation link
                    ViewBag.XAllo = "Registration successful. Please check your email to confirm your account.";
                    return View("Signup", model);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //public IActionResult RegistrationSuccess()
        //{
        //    // Retrieve the message from TempData and pass to the view if needed
        //    ViewBag.Message = TempData["Message"] as string;
        //    return View();
        //}
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home"); // or an error view
            }

            var result = await _userService.ConfirmEmailAsync(userId, token);
            if (result)
            {
                ViewBag.successConfirm = "success";
                ViewBag.Redirect = true;
                //logic to redirect user to homepage after 5 sec
                //return View("ConfirmEmailSuccess");
            }
            else
            {
                ViewBag.ErrorMessage = "failure";
            }

            return View("SignUp");
        }
        [HttpPost]
        public async Task<IActionResult> ResendConfirmationEmail()
        {
            var email = TempData["UserEmail"] as string; // Retrieve email from TempData

            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Error = "Could not retrieve email to resend the confirmation. Please try signing up again.";
                return View("SignUp");
            }
            try
            {
                await _userService.ResendConfirmationEmailAsync(email);
                ViewBag.Message = "Confirmation email resent successfully.";
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Failed to resend confirmation email: {ex.Message}";
            }
            return Ok();
            }
        #endregion

        #region forgotpass?

        [HttpGet]
        public IActionResult ForgotPasswordForm()
        {
            return PartialView("_ForgotPasswordForm");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var user = await _userInManager.FindByEmailAsync(model.Email);
                //if (user == null || !(await _userInManager.IsEmailConfirmedAsync(user)))
                //{
                //    // Optionally inform the user or just log internally
                //    return PartialView("ForgotPasswordConfirmation");
                //}

                //var token = await _userInManager.GeneratePasswordResetTokenAsync(user);
                //await _userService.SendResetPasswordEmailAsync(user.Email, token);
                //_logger.LogInformation("just about to redirect");
                return PartialView("_ForgotPasswordConfirmation");
            }

            return PartialView(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Retrieve user by email stored in a hidden field within the form
            var user = await _userInManager.FindByEmailAsync(model.email);
            if (user == null)
            {
                // Redirect to confirmation to avoid revealing user existence
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await _userInManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return View("Error"); // Show an error view if the code is missing
            }

            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }



        #endregion
    }
}
