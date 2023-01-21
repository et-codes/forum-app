using API.DTOs;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public CategoriesController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        public async Task<IEnumerable<CategoryDto>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            
            var result = _mapper.Map<List<CategoryDto>>(categories);

            return result;
        }
    }
}