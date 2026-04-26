using System.ComponentModel.DataAnnotations;

namespace AutiBuddy.DTOs
{
    public class ChildCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Range(1, 10)]
        public int Age { get; set; }

        [Required]
        public string Gender { get; set; }
    }
}
