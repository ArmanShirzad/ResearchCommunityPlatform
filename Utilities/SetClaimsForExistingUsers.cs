using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using ResearchCommunityPlatform.Models;

using System.Security.Claims;

namespace ResearchCommunityPlatform.Utilities
{
    public class SetClaimsForExistingUsers : IHostedService
    {
        private readonly UserManager<User> _userManager;

        public SetClaimsForExistingUsers(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        //public async Task StartAsync(CancellationToken cancellationToken)
        //{
        //    var users = await _userManager.Users.ToListAsync();
        //    foreach (var user in users)
        //    {
        //        await _userManager.AddClaimsAsync(user, new[] {
        //        new Claim(ClaimTypes., Claim.Equals (user.UserName)),
        //        new Claim(ClaimTypes.Email, user.Email),
        //    });
        //    }
        //}

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // No-op
        }
    }

}
