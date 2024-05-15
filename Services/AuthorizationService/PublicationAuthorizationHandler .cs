    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authorization.Infrastructure;
    using Microsoft.VisualBasic;

    using ResearchCommunityPlatform.Models;

    using System.Security.Claims;
    using System.Threading.Tasks;
namespace ResearchCommunityPlatform.Services.AuthorizationService
{

    public class PublicationAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Publication>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OperationAuthorizationRequirement requirement,
                                                       Publication resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (requirement.Name == AuthorizationConstants.CreateOperationName ||
                requirement.Name == AuthorizationConstants.ReadOperationName ||
                requirement.Name == AuthorizationConstants.UpdateOperationName ||
                requirement.Name == AuthorizationConstants.DeleteOperationName)
            {
                if (resource.UserId == context.User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }

}
