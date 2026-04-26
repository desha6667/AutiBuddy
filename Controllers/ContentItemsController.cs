using AutiBuddy.Data;
using AutiBuddy.DTOs;
using AutiBuddy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutiBuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContentItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/ContentItems
        [HttpPost]
        public async Task<IActionResult> Create(ContentItemCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name is required");

            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == dto.CategoryId);

            if (!categoryExists)
                return BadRequest("Invalid CategoryId");

            var item = new ContentItem
            {
                Name = dto.Name,
                ImageUrl = dto.ImageUrl,
                AudioUrl = dto.AudioUrl,
                CategoryId = dto.CategoryId
            };

            _context.ContentItems.Add(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        // GET: api/ContentItems
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _context.ContentItems
                .Include(i => i.Category)
                .ToListAsync();

            return Ok(items);
        }
        // GET: api/ContentItems/category/1
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var items = await _context.ContentItems
                .Where(i => i.CategoryId == categoryId)
                .ToListAsync();

            return Ok(items);
        }
    }
}
