using Core.Entities;

namespace API.DTOs
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public CategoryDto PostCategory { get; set; }
        public AuthorDto Author { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Views { get; set; }
        public int Replies { get; set; }
    }
}
