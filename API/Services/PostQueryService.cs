using Core.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public interface IPostQueryService
    {
        Task<IEnumerable<PostEntity>> GetAllPosts();
        Task<IEnumerable<PostEntity>> GetPostAndReplies(Guid id);
        Task<PostEntity> GetPost(Guid id);
    }

    public class PostQueryService : IPostQueryService
    {
        private readonly DataContext _context;

        public PostQueryService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PostEntity>> GetAllPosts()
        {
            // Returns all original posts for category view
            return await _context.Posts
                .Where(p => p.InReplyTo == null)
                .Include("PostCategory")
                .Include("Author")
                .OrderBy(post => post.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostEntity>> GetPostAndReplies(Guid id)
        {
            // Returns post and its replies for post view
            var result = await _context.Posts
                .Where(p => p.Id == id || p.InReplyTo.Id == id)
                .Include("PostCategory")
                .Include("Author")
                .Include("InReplyTo")
                .ToListAsync();

            UpdateViewCount(id);

            return result;
        }

        public async Task<PostEntity> GetPost(Guid id)
        {
            return await _context.Posts
                .Where(p => p.Id == id)
                .Include("PostCategory")
                .Include("Author")
                .Include("InReplyTo")
                .FirstOrDefaultAsync();
        }

        private async void UpdateViewCount(Guid id)
        {
            var post = await GetPost(id);
            post.Views += 1;
            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
