using AutiBuddy.Data;
using AutiBuddy.DTOs;
using AutiBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutiBuddy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ParentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/parents
        [HttpGet]
        public async Task<IActionResult> GetParents()
        {
            var parents = await _context.Parents
                                        .Include(p => p.Children)
                                        .ToListAsync();

            return Ok(parents);
        }

        // POST: api/parents
        [HttpPost]
        public async Task<IActionResult> CreateParent(ParentCreateDto dto)
        {
            var emailExists = await _context.Parents.AnyAsync(p => p.Email == dto.Email);
            if (emailExists)
            {
                return BadRequest("Email already exists");
            }
            // 🔴 Validate children
            foreach (var child in dto.Children)
            {
                if (string.IsNullOrWhiteSpace(child.Name))
                    return BadRequest("Child name is required");

                if (child.Age <= 0)
                    return BadRequest("Child age must be greater than zero");
            }

            var parent = new Parent
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RelationToChild = dto.RelationToChild,
                Children = dto.Children.Select(c => new Child
                {
                    Name = c.Name,
                    Age = c.Age,
                    Gender = c.Gender
                }).ToList()
            };

            _context.Parents.Add(parent);
            await _context.SaveChangesAsync();

            return Ok(new ParentResponseDto
            {
                Id = parent.Id,
                Name = parent.Name,
                Email = parent.Email
            });
        }
    }
}
