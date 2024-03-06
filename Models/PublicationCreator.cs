namespace ResearchCommunityPlatform.Models
{
    public class PublicationCreator
    {
        public int PublicationId { get; set; }
        public string UserID { get; set; } //fk
        public virtual Publication Publication { get; set; }//fk
        public virtual User User { get; set; } //navigation property
    }
}