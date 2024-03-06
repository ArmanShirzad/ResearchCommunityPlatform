using Microsoft.AspNetCore.Identity;

namespace ResearchCommunityPlatform.Models
{
    public class User : IdentityUser
    {
        public ICollection<PublicationCreator> PublicationCreators { get; set; }
    }
}
