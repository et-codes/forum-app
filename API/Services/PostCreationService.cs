using API.DTOs;
using Core.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace API.Services
{
    public interface IPostCreationService
    {
        Task<PostEntity> Create(PostDto post, UserEntity author, PostEntity inReplyTo);
        Task<PostEntity> Create(PostDto post, UserEntity author);
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

        public async Task<PostEntity> Create(PostDto post, UserEntity author, PostEntity inReplyTo)
        {
            CategoryEntity category;

            if (inReplyTo == null)
            {
                var categoryId = Guid.Parse(post.CategoryId);
                category = await _context.Categories.FindAsync(categoryId);
            }
            else
            {
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

        public async Task<PostEntity> Create(PostDto post, UserEntity author)
        {
            return await Create(post, author, null);
        }
    }
}