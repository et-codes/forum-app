using API.DTOs;
using AutoMapper;
using Core.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateCategory(CategoryDto category)
        {
            var newCategory = _mapper.Map<CategoryEntity>(category);
            newCategory.CreatedDate = DateTime.UtcNow;

            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            var newCategoryDto = _mapper.Map<CategoryDto>(newCategory);

            return StatusCode(StatusCodes.Status201Created, newCategoryDto);
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, CategoryDto updatedCategory)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null) return NotFound();

            category.Name = updatedCategory.Name;
            category.Description = updatedCategory.Description;
            category.ModifiedDate = DateTime.UtcNow;
            
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}