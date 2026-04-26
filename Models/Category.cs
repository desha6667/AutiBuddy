namespace AutiBuddy.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }   // Fruits, Animals, Vegetables

        public ICollection<ContentItem> ContentItems { get; set; }
    }
}
