using API.DTOs;
using Core.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public interface IPostCreationService
    {
        Task<PostEntity> Create(HttpContext httpContext, PostDto post, Guid? inReplyToId);
        Task<PostEntity> Create(HttpContext httpContext, PostDto post);
    }

    public class PostCreationService : IPostCreationService
    {
        private readonly DataContext _context;
        private readonly UserManager<UserEntity> _userManager;

        public PostCreationService(DataContext context, UserManager<UserEntity> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<PostEntity> Create(HttpContext httpContext, PostDto post, Guid? inReplyToId)
        {
            CategoryEntity category;
            PostEntity inReplyTo = null;

            var author = await _userManager.GetUserAsync(httpContext.User);

            if (inReplyToId == null)
            {
                var categoryId = Guid.Parse(post.CategoryId);
                category = await _context.Categories.FindAsync(categoryId);
            }
            else
            {
                inReplyTo = await _context.Posts
                    .Include("PostCategory")
                    .FirstOrDefaultAsync(p => p.Id == inReplyToId);

                if (inReplyTo == null) return null;

                category = inReplyTo.PostCategory;
            }

            var newPost = new PostEntity
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

        public async Task<PostEntity> Create(HttpContext httpContext, PostDto post)
        {
            return await Create(httpContext, post, null);
        }
    }
}