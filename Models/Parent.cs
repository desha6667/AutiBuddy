namespace AutiBuddy.Models
{
    public class Parent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // مبدئيًا من غير تشفير
        public string Password { get; set; }

        public string RelationToChild { get; set; }

        public List<Child> Children { get; set; }
    }
}
