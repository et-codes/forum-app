using API.DTOs;
using API.Services;
using Core.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// api/posts        GET all posts that are not replies
//                  POST new post [Authorize]
// api/posts/id     GET single post and all its replies
//                  POST reply to post id [Authorize]
//                  PUT edit post [Authorize]
//                  DELETE delete post and its replies [Authorize]

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PostsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IPostQueryService _postQueryService;
        private readonly IPostCreationService _postCreationService;
        private readonly IPostDeletionService _postDeletionService;

        public PostsController(
            DataContext context, 
            UserManager<UserEntity> userManager,
            IPostQueryService postQueryService,
            IPostCreationService postCreationService,
            IPostDeletionService postDeletionService
        )
        {
            _context = context;
            _userManager = userManager;
            _postQueryService = postQueryService;
            _postCreationService = postCreationService;
            _postDeletionService = postDeletionService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PostEntity), StatusCodes.Status200OK)]
        public async Task<IEnumerable<PostEntity>> GetAllPosts()
        {
            return await _postQueryService.GetAllPosts();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PostEntity), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPost(Guid id)
        {
            var result = await _postQueryService.GetPostAndReplies(id);

            return result == null ? NotFound() : Ok(result);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreatePost(PostDto post)
        {
            var newPost = await _postCreationService.Create(HttpContext, post);

            return CreatedAtAction(nameof(GetPost), new {id = newPost.Id}, newPost);
        }

        [Authorize]
        [HttpPost("{inReplyToId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReplyToPost(Guid inReplyToId, PostDto post)
        {
            var newPost = await _postCreationService.Create(HttpContext, post, inReplyToId);

            if (newPost == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetPost), new {id = newPost.Id}, newPost);
        }

        // [Authorize]
        // [HttpPut("{id}")]

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await _postDeletionService.Delete(HttpContext, _postQueryService, id);
        }
    }
}