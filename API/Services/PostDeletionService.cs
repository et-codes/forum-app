using Core.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public interface IPostDeletionService
    {
        Task<IActionResult> Delete(IEnumerable<PostEntity> postsToDelete);
    }

    public class PostDeletionService : IPostDeletionService
    {
        private readonly DataContext _context;

        public PostDeletionService(DataContext context)
        {
            _context = context;
        }

        public DataContext Context { get; }

        public async Task<IActionResult> Delete(IEnumerable<PostEntity> postsToDelete)
        {
            StatusCodeResult result;

            if (postsToDelete == null)
            {
                result = new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            else
            {
                _context.Posts.RemoveRange(postsToDelete);
                await _context.SaveChangesAsync();
                result = new StatusCodeResult(StatusCodes.Status204NoContent);
            }

            return result;
        }
    }
}
