using Core.Entities;
using Infrastructure;

namespace API.Services
{
    public interface IPostDeletionService
    {
        Task<PostEntity> Delete(Guid id);
    }

    public class PostDeletionService : IPostDeletionService
    {
        private readonly DataContext _context;

        public PostDeletionService(DataContext context)
        {
            _context = context;
        }

        public DataContext Context { get; }

        public async Task<PostEntity> Delete(Guid id)
        {
            var postToDelete = await _context.Posts.FindAsync(id);
            if (postToDelete != null)
            {
                _context.Posts.Remove(postToDelete);
                await _context.SaveChangesAsync();
            }

            return postToDelete;
        }
    }
}
