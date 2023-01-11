namespace Core.Models
{
  public class Post : BaseModel
    {
        public Category PostCategory {get; set; }
        public User Author { get; set; }
        public Post? InReplyTo { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Views { get; set; } = 1;
    }
}