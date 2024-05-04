using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using ResearchCommunityPlatform.Controllers;
using System.Security.Policy;
using ResearchCommunityPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
namespace ResearchCommunityPlatform.Services.UserSevice
{
    public class UserService
        :IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly AppDbContext _context;

        public UserService(UserManager<User> userManager,
                           IEmailSender emailSender,
                           IHttpContextAccessor httpContextAccessor,
                           IUrlHelperFactory urlHelperFactory,
                           IActionContextAccessor actionContextAccessor,
                           AppDbContext context,
                           SignInManager<User> signInManager) // Optionally inject IActionContextAccessor if needed
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor; // This is optional and may not be necessary
            _context = context;
            _signInManager = signInManager;
        }


        public async Task<RegistrationResult> RegisterUserAsync(RegisterViewModel model)
        {
        
            var user = new User { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return new RegistrationResult
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var actionContext = _actionContextAccessor.ActionContext;
            var urlHelper = _urlHelperFactory.GetUrlHelper(actionContext);

            var confirmationLink = urlHelper.Action("ConfirmEmail", "Account", new
            {
                userId = user.Id,
                token = token
            }, _httpContextAccessor.HttpContext.Request.Scheme);

            // Assuming you have implemented SendConfirmationEmailAsync
            await SendConfirmationEmailAsync(user.Email, confirmationLink);

            return new RegistrationResult { Success = true };
        }


        
        public async Task SendConfirmationEmailAsync(string email, string confirmationLink)
        {
            string subject = "Confirm your email";
            string message = $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>";
            await _emailSender.SendEmailAsync(email, subject, message);
        }
        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        public bool UsernameExists(string username)
        {
            return _context.Users.Any(u => u.UserName == username);
        }

        public  bool EmailExists(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            var state = _context.Users.Any(u => u.Email == email);
            return state; 
        }

        public async Task<bool> LoginUserAsync(string username, string password)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
                    return result.Succeeded;
                }
            }
            catch (Exception)
            {

                throw;
            }
     
            return false;
        }
    }
}
