namespace Core.Entities
{
    public class PostEntity : BaseEntity
    {
        public CategoryEntity PostCategory { get; set; }
        public UserEntity Author { get; set; }
        public PostEntity InReplyTo { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Views { get; set; }
        public int Replies { get; set; }
    }
}