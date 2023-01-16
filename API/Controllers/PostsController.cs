using API.DTOs;
using API.Services;
using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// api/posts        GET all posts
//                  POST new post [Authorize]
// api/posts/id     GET single post
//                  POST reply to post id [Authorize]
//                  PUT edit post [Authorize]
//                  DELETE delete post [Authorize]

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PostsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IPostCreationService _postCreationService;

        public PostsController(
            DataContext context, 
            UserManager<User> userManager,
            IPostCreationService postCreationService
        )
        {
            _context = context;
            _userManager = userManager;
            _postCreationService = postCreationService;
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
            var author = await _userManager.GetUserAsync(HttpContext.User);

            var newPost = _postCreationService.Create(post, author);
            return CreatedAtAction(nameof(GetPost), 
                new {id = newPost.Id}, newPost);
        }

        [Authorize]
        [HttpPost("{inReplyToId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReplyToPost(Guid inReplyToId, PostDto post)
        {
            var author = await _userManager.GetUserAsync(HttpContext.User);

            var newPost = _postCreationService.Create(post, author, inReplyToId);
            return CreatedAtAction(nameof(GetPost), 
                new {id = newPost.Id}, newPost);
        }

        // [Authorize]
        // [HttpPut("{id}")]

        // [Authorize]
        // [HttpDelete("{id}")]
    }
}