using API.DTOs;
using API.Services;
using Core.Entities;
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
        private readonly UserManager<UserEntity> _userManager;
        private readonly IPostCreationService _postCreationService;
        private readonly IPostDeletionService _postDeletionService;

        public PostsController(
            DataContext context, 
            UserManager<UserEntity> userManager,
            IPostCreationService postCreationService,
            IPostDeletionService postDeletionService
        )
        {
            _context = context;
            _userManager = userManager;
            _postCreationService = postCreationService;
            _postDeletionService = postDeletionService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PostEntity), StatusCodes.Status200OK)]
        public async Task<IEnumerable<PostEntity>> GetPosts()
        {
            return await _context.Posts
                .Include("PostCategory")
                .Include("Author")
                .OrderBy(post => post.CreatedDate)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PostEntity), StatusCodes.Status200OK)]
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
            var inReplyTo = await _context.Posts
                .Include("PostCategory")
                .FirstOrDefaultAsync(p => p.Id == inReplyToId);
            if (inReplyTo == null) return NotFound();

            var author = await _userManager.GetUserAsync(HttpContext.User);

            var newPost = _postCreationService.Create(post, author, inReplyTo);
            return CreatedAtAction(nameof(GetPost), 
                new {id = newPost.Id}, newPost);
        }

        // [Authorize]
        // [HttpPut("{id}")]

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedPost = await _postDeletionService.Delete(id);
            return deletedPost == null ? NotFound() : NoContent();
        }
    }
}