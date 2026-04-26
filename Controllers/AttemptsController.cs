using AutiBuddy.Data;
using AutiBuddy.DTOs;
using AutiBuddy.Helpers;
using AutiBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace AutiBuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttemptsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AttemptsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Attempts
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateAttempt([FromForm] AttemptUploadDto dto)
        {
            if (dto.File == null)
                return BadRequest("Audio file is required");

            var child = await _context.Children.FindAsync(dto.ChildId);
            if (child == null)
                return BadRequest("Invalid ChildId");

            var contentItem = await _context.ContentItems.FindAsync(dto.ContentItemId);
            if (contentItem == null)
                return BadRequest("Invalid ContentItemId");

            using var client = new HttpClient();
            using var content = new MultipartFormDataContent();

            var stream = dto.File.OpenReadStream();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("audio/wav");

            content.Add(fileContent, "file", dto.File.FileName);

            var response = await client.PostAsync("http://127.0.0.1:5000/recognize", content);

            if (!response.IsSuccessStatusCode)
                return StatusCode(500, "Speech service error");

            var voskResult = await response.Content.ReadAsStringAsync();

            var json = System.Text.Json.JsonDocument.Parse(voskResult);
            var recognizedText = json.RootElement.GetProperty("text").GetString() ?? "";

            double similarity = LevenshteinHelper.CalculateSimilarity(
                recognizedText,
                contentItem.Name
            );

            var previousSuccessCount = await _context.Attempts
                .CountAsync(a =>
                    a.ChildId == dto.ChildId &&
                    a.ContentItemId == dto.ContentItemId &&
                    a.IsSuccessful);

            double threshold = 50 + (previousSuccessCount * 10);

            if (threshold > 90)
                threshold = 90;

            bool isSuccessful = similarity >= threshold;

            var attempt = new Attempt
            {
                ChildId = dto.ChildId,
                ContentItemId = dto.ContentItemId,
                RecognizedText = recognizedText,
                SimilarityScore = similarity,
                IsSuccessful = isSuccessful
            };

            _context.Attempts.Add(attempt);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                recognizedText,
                similarity,
                success = isSuccessful
            });
        }
    }
}