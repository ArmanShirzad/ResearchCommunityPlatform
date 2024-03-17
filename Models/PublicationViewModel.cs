using System.ComponentModel.DataAnnotations;

namespace ResearchCommunityPlatform.Models
{
    public class PublicationViewModel
    {
            [Key]
            public int PublicationId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string DateOfPublish { get; set; }
            public string UploadDate { get; set; }
            public string PubType { get; set; }
            public string DOI { get; set; }
            public List<string> Authors { get; set; }
            public List<FileViewModel> Files { get; set; }


        //public string Title { get; set; }
        //public string Authors { get; set; }
        //public string DOI { get; set; }
        //public string PublicationDate { get; set; }
        //public string Abstract { get; set; }
        //public string Keywords { get; set; }



    }
}
