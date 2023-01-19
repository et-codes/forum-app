using API.DTOs;
using Core.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public interface IPostUpdateService
    {
        Task<IActionResult> Update(Guid id, NewPostDto postToUpdate);
    }

    public class PostUpdateService : IPostUpdateService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<UserEntity> _userManager;

        public PostUpdateService(DataContext context, IHttpContextAccessor accessor, 
            UserManager<UserEntity> userManager)
        {
            _context = context;
            _accessor = accessor;
            _userManager = userManager;
        }

        public async Task<IActionResult> Update(Guid id, NewPostDto updatedPost) 
        {
            var post = await _context.Posts
                .Where(p => p.Id == id)
                .Include("Author")
                .FirstOrDefaultAsync();

            if (post == null) 
            { 
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }

            var updatingUser = await _userManager.GetUserAsync(_accessor.HttpContext.User);

            if (updatingUser != post.Author)
            {
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }

            post.Text = updatedPost.Text;
            post.ModifiedDate = DateTime.UtcNow;

            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }
    }
}
