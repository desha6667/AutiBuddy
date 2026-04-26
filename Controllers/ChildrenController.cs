using AutiBuddy.Data;
using AutiBuddy.DTOs;
using AutiBuddy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutiBuddy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChildrenController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChildrenController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/children/parent/1
        [HttpGet("parent/{parentId}")]
        public async Task<IActionResult> GetChildrenByParent(int parentId)
        {
            var children = await _context.Children
                .Where(c => c.ParentId == parentId)
                .ToListAsync();

            return Ok(children);
        }
        // PUT: api/children/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChild(int id, ChildUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var child = await _context.Children.FindAsync(id);
            if (child == null)
                return NotFound("Child not found");
            if (dto.Age <= 0)
                return BadRequest("Age must be greater than zero");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name is required");

            child.Name = dto.Name;
            child.Age = dto.Age;
            child.Gender = dto.Gender;

            await _context.SaveChangesAsync();

            return Ok("Child updated successfully");
        }
        // DELETE: api/children/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChild(int id)
        {
            var child = await _context.Children.FindAsync(id);
            if (child == null)
                return NotFound("Child not found");

            _context.Children.Remove(child);
            await _context.SaveChangesAsync();

            return Ok("Child deleted successfully");
        }
        // GET: api/children/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChildById(int id)
        {
            var child = await _context.Children.FindAsync(id);

            if (child == null)
                return NotFound("Child not found");

            return Ok(child);
        }
    }
}
