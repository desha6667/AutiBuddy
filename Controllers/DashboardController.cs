using AutiBuddy.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutiBuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Dashboard/child/1/overview
        [HttpGet("child/{childId}/overview")]
        public async Task<IActionResult> GetChildOverview(int childId)
        {
            var attempts = await _context.Attempts
                .Where(a => a.ChildId == childId)
                .ToListAsync();

            int totalAttempts = attempts.Count;
            int successfulAttempts = attempts.Count(a => a.IsSuccessful);

            double successRate = totalAttempts == 0
                ? 0
                : (double)successfulAttempts / totalAttempts * 100;

            return Ok(new
            {
                totalAttempts,
                successfulAttempts,
                successRate
            });
        }
        // GET: api/Dashboard/child/1/attempts
        [HttpGet("child/{childId}/attempts")]
        public async Task<IActionResult> GetChildAttempts(int childId)
        {
            var attempts = await _context.Attempts
                .Include(a => a.ContentItem)
                .Where(a => a.ChildId == childId)
                .OrderByDescending(a => a.CreatedAt)
                .Take(20)
                .Select(a => new
                {
                    word = a.ContentItem.Name,
                    recognizedText = a.RecognizedText,
                    similarity = a.SimilarityScore,
                    success = a.IsSuccessful,
                    date = a.CreatedAt
                })
                .ToListAsync();

            return Ok(attempts);
        }
        // GET: api/Dashboard/child/1/categories
        [HttpGet("child/{childId}/categories")]
        public async Task<IActionResult> GetCategoryProgress(int childId)
        {
            var data = await _context.Attempts
                .Include(a => a.ContentItem)
                .ThenInclude(ci => ci.Category)
                .Where(a => a.ChildId == childId)
                .GroupBy(a => a.ContentItem.Category.Name)
                .Select(g => new
                {
                    category = g.Key,
                    successRate = g.Count(a => a.IsSuccessful) * 100.0 / g.Count()
                })
                .ToListAsync();

            return Ok(data);
        }
    }
}
