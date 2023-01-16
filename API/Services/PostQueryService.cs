using Core.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public interface IPostQueryService
    {
        Task<IEnumerable<PostEntity>> GetAllPosts();
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
            return await _context.Posts
                .Where(p => p.InReplyTo == null)
                .Include("PostCategory")
                .Include("Author")
                .OrderBy(post => post.CreatedDate)
                .ToListAsync();
        }

        public async Task<PostEntity> GetPost(Guid id)
        {
            return await _context.Posts
                .Include("PostCategory")
                .Include("Author")
                .Include("InReplyTo")
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
