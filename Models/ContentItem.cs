namespace AutiBuddy.Models
{
    public class ContentItem
    {
        public int Id { get; set; }

        public string Name { get; set; }        // Apple
        public string ImageUrl { get; set; }    // apple.png
        public string AudioUrl { get; set; }    // apple.mp3

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
