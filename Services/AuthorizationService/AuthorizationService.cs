using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;

using ResearchCommunityPlatform.Models;

using System.Security.Claims;

namespace ResearchCommunityPlatform.Services.AuthorizationService
{
    public class AuthorizationService
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<User> _userManager;

        public AuthorizationService(IAuthorizationService authorizationService, UserManager<User> userManager)
        {
            _authorizationService = authorizationService;
            _userManager = userManager;
        }
        public async Task<bool> CanEditPublicationAsync(ClaimsPrincipal user, Publication publication)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync( user, publication, AuthorizationConstants.Update);
            return authorizationResult.Succeeded;
        }
    }
}
