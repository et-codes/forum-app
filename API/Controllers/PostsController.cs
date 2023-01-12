using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly DataContext _context;

        public PostsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Post), StatusCodes.Status200OK)]
        public async Task<IEnumerable<Post>> Get()
        {
            return await _context.Posts
                .OrderBy(post => post.CreatedDate)
                .ToListAsync();
        }
    }
}