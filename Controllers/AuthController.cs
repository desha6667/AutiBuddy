using AutiBuddy.Data;
using AutiBuddy.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutiBuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Email and password are required");

            var parent = await _context.Parents
                .FirstOrDefaultAsync(p => p.Email == dto.Email);

            if (parent == null)
                return Unauthorized("Invalid email or password");

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, parent.Password);

            if (!isPasswordValid)
                return Unauthorized("Invalid email or password");

            return Ok(new LoginResponseDto
            {
                Id = parent.Id,
                Name = parent.Name,
                Email = parent.Email
            });
        }
    }
}
