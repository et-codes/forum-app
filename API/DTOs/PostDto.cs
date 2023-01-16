namespace API.DTOs
{
  public class PostDto
    {
        public string CategoryId { get; set; }
        public string InReplyToId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}