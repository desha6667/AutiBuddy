namespace AutiBuddy.Models
{
    public class Child
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }

        public int ParentId { get; set; }
        public Parent Parent { get; set; }
    }
}
