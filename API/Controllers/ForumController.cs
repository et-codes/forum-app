using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumController : ControllerBase
    {
        private readonly DataContext _context;

        public ForumController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("posts")]
        [ProducesResponseType(typeof(Post), StatusCodes.Status200OK)]
        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await _context.Posts
                .OrderBy(post => post.CreatedDate)
                .ToListAsync();
        }

        [HttpGet("categories")]
        [ProducesResponseType(typeof(Post), StatusCodes.Status200OK)]
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}