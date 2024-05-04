using ResearchCommunityPlatform.Services.UserSevice;

using System.ComponentModel.DataAnnotations;

namespace ResearchCommunityPlatform.Utilities
{
    public class EmailValidationAttribute : ValidationAttribute
    {

        public EmailValidationAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var email = value as string;
            if (string.IsNullOrEmpty(email))
            {
                return ValidationResult.Success; // Skip validation if no email is provided
            }

            // Resolve the service that checks if the email exists
            var userService = (IUserService)validationContext.GetService(typeof(IUserService));

            if (userService == null)
            {
                throw new Exception("UserService not found. It must be registered in the DI container.");
            }

            bool exists = userService.EmailExists(email);
            if (exists)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return "Email is already in use.";
        }

    }
}
