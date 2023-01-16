using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public interface IPostDeletionService
    {
        Task<IActionResult> Delete(Guid id);
    }

    public class PostDeletionService : IPostDeletionService
    {
        private readonly DataContext _context;

        public PostDeletionService(DataContext context)
        {
            _context = context;
        }

        public DataContext Context { get; }

        public async Task<IActionResult> Delete(Guid id)
        {
            StatusCodeResult result;

            var postToDelete = await _context.Posts.FindAsync(id);

            if (postToDelete == null)
            {
                result = new StatusCodeResult(StatusCodes.Status204NoContent);
            }
            else
            {
                _context.Posts.Remove(postToDelete);
                await _context.SaveChangesAsync();
                result = new StatusCodeResult(StatusCodes.Status404NotFound);
            }

            return result;
        }
    }
}
