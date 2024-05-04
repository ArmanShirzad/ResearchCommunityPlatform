using ResearchCommunityPlatform.Controllers;
using ResearchCommunityPlatform.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ResearchCommunityPlatform.Models; // Ensure this is where your ViewModel and user model are defined

namespace ResearchCommunityPlatform.Services.UserSevice
{
    public interface IUserService
    {
        /// <summary>
        /// Registers a new user asynchronously.
        /// </summary>
        /// <param name="model">The registration view model containing user data.</param>
        /// <returns>A task result containing registration success status and potential errors.</returns>
        Task<RegistrationResult> RegisterUserAsync(RegisterViewModel model);

        /// <summary>
        /// Sends a confirmation email to the user.
        /// </summary>
        /// <param name="email">The email address to send the confirmation to.</param>
        /// <param name="confirmationLink">The link to include in the confirmation email for user verification.</param>
        /// <returns>A task that represents the asynchronous send operation.</returns>
        Task SendConfirmationEmailAsync(string email, string confirmationLink);

        /// <summary>
        /// Confirms the user's email based on the token received.
        /// </summary>
        /// <param name="userId">The user's unique identifier.</param>
        /// <param name="token">The token to validate email confirmation.</param>
        /// <returns>A task result indicating the success or failure of the email confirmation.</returns>
        Task<bool> ConfirmEmailAsync(string userId, string token);

        bool UsernameExists(string u);
        bool EmailExists(string email);

         Task<bool> LoginUserAsync(string email, string password);


    }

}