using System.ComponentModel.DataAnnotations;

namespace ResearchCommunityPlatform.Models
{
    public class File
    {
        [Key]
        public int FileID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; } // To store the location or URL of the file
        public float Size { get; set; } //mb
        public int PublicationId { get; set; } //fk
        public virtual Publication Publication { get; set; } //NAV PROPERTY

    }
}
