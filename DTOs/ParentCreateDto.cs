using System.ComponentModel.DataAnnotations;

namespace AutiBuddy.DTOs
{
    public class ParentCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string RelationToChild { get; set; }

        [Required]
        public List<ChildCreateDto> Children { get; set; }
    }
}
