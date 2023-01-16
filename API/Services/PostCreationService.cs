using API.DTOs;
using Core.Models;
using Infrastructure;

namespace API.Services
{
  public interface IPostCreationService
    {
        Task<Post> Create(PostDto post, User author, Guid? inReplyToId);
        Task<Post> Create(PostDto post, User author);
    }

    public class PostCreationService : IPostCreationService
    {
        private readonly DataContext _context;

        public PostCreationService(DataContext context)
        {
            _context = context;
        }

        public async Task<Post> Create(PostDto post, User author, Guid? inReplyToId)
        {
            Category category;
            Post inReplyTo;

            if(inReplyToId == null)
            {
                inReplyTo = null;
                var categoryId = Guid.Parse(post.CategoryId);
                category = await _context.Categories.FindAsync(categoryId);
            }
            else
            {
                inReplyTo = await _context.Posts.FindAsync(inReplyToId);
                category = inReplyTo.PostCategory;
            }

            var newPost = new Post
            {
                CreatedDate = DateTime.UtcNow,
                PostCategory = category,
                Author = author,
                InReplyTo = inReplyTo,
                Title = post.Title,
                Text = post.Text,
            };

            await _context.Posts.AddAsync(newPost);
            await _context.SaveChangesAsync();

            return newPost;
        }

        public async Task<Post> Create(PostDto post, User author)
        {
            return await Create(post, author, null);
        }
    }
}