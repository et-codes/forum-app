using Core.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoriesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PostEntity), StatusCodes.Status200OK)]
        public async Task<IEnumerable<CategoryEntity>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}