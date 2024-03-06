using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ResearchCommunityPlatform.Models
{
    public class Publication
    {
        [Key]
        public int PublicationId { get; set; }
        public int UserId { get; set; } //fk
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [DisplayName("DOI")]
        public long DOI { get; set; }
        public string Description { get; set; } // first 30 words of the abstract

        public string DateOfPublish { get; set; }
        public string UploadDate { get; set; }
        public string PubType { get; set; } //journal or conferance
        public virtual User User { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<PublicationCreator> PublicationCreators { get; set; }


        // a lit of authors



        /*
         Title
            Creators (This should be a navigation property to a new join table that links records to users)
            Description
            DOI (Digital Object Identifier)
            UploadDate
            List<File> Files (Navigation property)
            CommunityID (FK)
            Community (Navigation property)
         */

    }

}
