namespace AutiBuddy.Models
{
    public class Attempt
    {
        public int Id { get; set; }

        public int ChildId { get; set; }
        public Child Child { get; set; }

        public int ContentItemId { get; set; }
        public ContentItem ContentItem { get; set; }

        public string RecognizedText { get; set; }   // اللي الطفل نطقه
        public double SimilarityScore { get; set; }  // نسبة التشابه (0 → 100
        public bool IsSuccessful { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
