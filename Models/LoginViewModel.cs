using System.ComponentModel.DataAnnotations;

namespace ResearchCommunityPlatform.Models
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        //public string? ReturnUrl { get; set; }
    }
}