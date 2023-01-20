using Core.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public interface IPostDeletionService
    {
        Task<IActionResult> Delete(Guid id);
    }

    public class PostDeletionService : IPostDeletionService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<UserEntity> _userManager;

        public PostDeletionService(DataContext context, UserManager<UserEntity> userManager,
            IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
            _userManager = userManager;
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var post = await _context.Posts
                .Where(p => p.Id == id)
                .Include("Author")
                .Include("InReplyTo")
                .FirstOrDefaultAsync();

            if (post == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }

            var deletingUser = await _userManager.GetUserAsync(_accessor.HttpContext.User);

            if (deletingUser != post.Author)
            {
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }

            if (post.InReplyTo != null)
            {
                var topicPost = post.InReplyTo;
                topicPost.Replies -= 1;
                _context.Entry(topicPost).State = EntityState.Modified;
            }

            var postsToDelete = await _context.Posts
                .Where(p => p.Id == id || p.InReplyTo.Id == id)
                .ToListAsync();

            _context.Posts.RemoveRange(postsToDelete);
            await _context.SaveChangesAsync();

            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }
    }
}
