using Core.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public interface IPostDeletionService
    {
        Task<IActionResult> Delete(HttpContext httpContext, IPostQueryService postQueryService, Guid id);
    }

    public class PostDeletionService : IPostDeletionService
    {
        private readonly DataContext _context;
        private readonly UserManager<UserEntity> _userManager;

        public PostDeletionService(DataContext context, UserManager<UserEntity> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Delete(HttpContext httpContext, 
            IPostQueryService postQueryService, Guid id)
        {
            StatusCodeResult result;

            var postToDelete = await postQueryService.GetPost(id);
            bool isAuthorized = await userIsPostAuthor(postToDelete, httpContext);

            if (!isAuthorized)
            {
                result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
            else if (postToDelete == null)
            {
                result = new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            else
            {
                var postsToDelete = await postQueryService.GetPostAndReplies(id);

                _context.Posts.RemoveRange(postsToDelete);
                await _context.SaveChangesAsync();

                result = new StatusCodeResult(StatusCodes.Status204NoContent);
            }

            return result;
        }

        private async Task<bool> userIsPostAuthor(PostEntity post, HttpContext httpContext)
        {
            var deletingUser = await _userManager.GetUserAsync(httpContext.User);
            return deletingUser == post.Author;
        }
    }
}
