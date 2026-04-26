namespace AutiBuddy.DTOs
{
    public class AttemptUploadDto
    {
        public int ChildId { get; set; }
        public int ContentItemId { get; set; }
        public IFormFile File { get; set; }
    }
}
