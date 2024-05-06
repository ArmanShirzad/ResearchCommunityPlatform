using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace ResearchCommunityPlatform.Models
{
    public class ForgetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
