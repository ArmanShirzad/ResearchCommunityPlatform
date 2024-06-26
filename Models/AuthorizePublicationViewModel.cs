using System.ComponentModel.DataAnnotations;

namespace ResearchCommunityPlatform.Models
{
    public class AuthorizePublicationViewModel
    {
        public int PublicationId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DateOfPublish { get; set; }
        public string UploadDate { get; set; }
        public string PubType { get; set; }
        public string DOI { get; set; }
        public List<string> Authors { get; set; }
        public List<FileViewModel> Files { get; set; }
        public bool CanEdit { get; set; } // Added to indicate if the user can edit
    }
        public class CreatePublicationViewModel
    {
        public int PublicationId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DateOfPublish { get; set; }
        public string PubType { get; set; }
        public string DOI { get; set; }
        public List<string> Authors { get; set; }
        public List<IFormFile> Files { get; set; }
    }

}
