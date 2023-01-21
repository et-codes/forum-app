using API.DTOs;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// api/posts        GET all topics (no replies)
//                  POST new post [Authorize]
// api/posts/id     GET topic post and its replies
//                  POST reply to topic post id [Authorize]
//                  PUT edit post [Authorize]
//                  DELETE delete post and its replies, if any [Authorize]

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PostsController : ControllerBase
    {
        private readonly IPostQueryService _postQueryService;
        private readonly IPostCreationService _postCreationService;
        private readonly IPostUpdateService _postUpdateService;
        private readonly IPostDeletionService _postDeletionService;
        private readonly IMapper _mapper;

        public PostsController(
            IPostQueryService postQueryService,
            IPostCreationService postCreationService,
            IPostUpdateService postUpdateService,
            IPostDeletionService postDeletionService,
            IMapper mapper)
        {
            _postQueryService = postQueryService;
            _postCreationService = postCreationService;
            _postUpdateService = postUpdateService;
            _postDeletionService = postDeletionService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
        public async Task<IEnumerable<PostDto>> GetTopics()
        {
            var topics = await _postQueryService.GetTopics();

            var result = _mapper.Map<List<PostDto>>(topics);

            return result;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTopicAndReplies(Guid id)
        {
            var posts = await _postQueryService.GetTopicAndReplies(id);

            var result = _mapper.Map<List<PostDto>>(posts);

            return result == null ? NotFound() : Ok(result);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreatePost(NewPostDto post)
        {
            var newPost = await _postCreationService.Create(post);
            var newPostDto = _mapper.Map<PostDto>(newPost);

            return StatusCode(StatusCodes.Status201Created, newPostDto);
        }

        [Authorize]
        [HttpPost("{inReplyToId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateReply(Guid inReplyToId, NewPostDto post)
        {
            var newPost = await _postCreationService.Create(post, inReplyToId);
            var newPostDto = _mapper.Map<PostDto>(newPost);

            if (newPost == null)
            {
                return NotFound();
            }

            return StatusCode(StatusCodes.Status201Created, newPostDto);
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, NewPostDto updatedPost)
        {
            return await _postUpdateService.Update(id, updatedPost);
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await _postDeletionService.Delete(id);
        }
    }
}