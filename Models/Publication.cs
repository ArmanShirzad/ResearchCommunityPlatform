using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Newtonsoft.Json;

namespace ResearchCommunityPlatform.Models
{
    public class Publication
    {
        [Key]
        public int PublicationId { get; set; }
        public string UserId { get; set; } //fk
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [DisplayName("DOI")]
        public string DOI { get; set; }
        public string Description { get; set; } // first 30 words of the abstract
        public string AuthorsSerialized { get; set; } // JSON serialized list of authors

        [NotMapped] // This attribute prevents EF Core from trying to map this property to the database.
        public List<string> Authors
        {
            get => string.IsNullOrEmpty(AuthorsSerialized) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(AuthorsSerialized);
            set => AuthorsSerialized = JsonConvert.SerializeObject(value);
        }
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
