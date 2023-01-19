using API.DTOs;
using Core.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public interface IPostCreationService
    {
        Task<PostEntity> Create(NewPostDto post, Guid? inReplyToId);
        Task<PostEntity> Create(NewPostDto post);
    }

    public class PostCreationService : IPostCreationService
    {
        private readonly DataContext _context;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IHttpContextAccessor _accessor;

        public PostCreationService(DataContext context, UserManager<UserEntity> userManager,
            IHttpContextAccessor accessor)
        {
            _context = context;
            _userManager = userManager;
            _accessor = accessor;
        }

        public async Task<PostEntity> Create(NewPostDto post, Guid? inReplyToId)
        {
            CategoryEntity category;
            PostEntity inReplyTo = null;

            var author = await _userManager.GetUserAsync(_accessor.HttpContext.User);

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

        public async Task<PostEntity> Create(NewPostDto post)
        {
            return await Create(post, null);
        }
    }
}