namespace ResearchCommunityPlatform.Models
{
    public class File
    {
        public int FileID { get; set; }
        // Other properties
        public int PublicationId { get; set; } //fk
        public virtual Publication Publication { get; set; } //NAV PROPERTY
    }
}
