
using API.DTOs;
using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// Add [Authorize] identifier above methods that need a token.

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PostsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public PostsController(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Post), StatusCodes.Status200OK)]
        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await _context.Posts
                .Include("PostCategory")
                .Include("Author")
                .OrderBy(post => post.CreatedDate)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Post), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPost(Guid id)
        {
            var post = await _context.Posts
                .Include("PostCategory")
                .Include("Author")
                .Include("InReplyTo")
                .FirstOrDefaultAsync(p => p.Id == id);
            return post == null ? NotFound() : Ok(post);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreatePost(PostDto post)
        {
            Post inReplyTo = null;
            bool isReply = Guid.TryParse(post.InReplyToId, out Guid inReplyToId);
            if (isReply)
            {
                inReplyTo = await _context.Posts.FindAsync(inReplyToId);
            }

            var categoryId = Guid.Parse(post.CategoryId);
            var category = await _context.Categories.FindAsync(categoryId);
            var author = await _userManager.GetUserAsync(HttpContext.User);

            var newPost = new Post
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

            return CreatedAtAction(nameof(GetPost), 
                new {id = newPost.Id}, newPost);
        }

        // [Authorize]
        // [HttpPut("{id}")]

        // [Authorize]
        // [HttpDelete]
    }
}