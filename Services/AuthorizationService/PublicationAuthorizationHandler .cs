using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using ResearchCommunityPlatform.Models;
using ResearchCommunityPlatform.Services.AuthorizationService;

using System.Threading.Tasks;

public class PublicationAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Publication>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<PublicationAuthorizationHandler> _logger;

    public PublicationAuthorizationHandler(UserManager<User> userManager, ILogger<PublicationAuthorizationHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Publication resource)
    {
        if (context.User == null || resource == null)
        {
            _logger.LogWarning("Authorization failed: User or resource is null.");
            return Task.CompletedTask;
        }

        if (requirement.Name != AuthorizationConstants.UpdateOperationName &&
            requirement.Name != AuthorizationConstants.ReadOperationName)
        {
            _logger.LogWarning($"Authorization failed: Unsupported requirement {requirement.Name}.");
            return Task.CompletedTask;
        }

        // Check if the user is the owner of the publication
        var userId = _userManager.GetUserId(context.User);
        if (resource.UserId == userId)
        {
            _logger.LogInformation($"Authorization succeeded: User {userId} is the owner of the publication.");
            context.Succeed(requirement);
        }
        else
        {
            _logger.LogWarning($"Authorization failed: User {userId} is not the owner of the publication.");
        }

        return Task.CompletedTask;
    }
}
