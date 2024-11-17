using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;

namespace UrlShortener.Services
{
    public class UrlService : IUrlService
    {
        private readonly ApplicationDbContext _context;

        public UrlService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> ShortenUrlAsync(string originalUrl, string? customAlias = null)
        {
            if (customAlias != null && customAlias.Length != 8)
            {
                throw new Exception("Custom alias must be exactly 8 characters long.");
            }

            var shortUrl = customAlias ?? GenerateShortUrl();

            // Check if the custom alias already exists
            if (customAlias != null)
            {
                var existingMapping = await _context.UrlMappings
                    .FirstOrDefaultAsync(u => u.ShortUrl == customAlias);
                if (existingMapping != null)
                {
                    throw new Exception("Custom alias already exists. Please choose a different alias.");
                }
            }

            var urlMapping = new UrlMapping
            {
                OriginalUrl = originalUrl,
                ShortUrl = shortUrl,
                CreatedAt = DateTime.UtcNow
            };

            _context.UrlMappings.Add(urlMapping);
            await _context.SaveChangesAsync();
            return shortUrl;
        }

        public async Task<string?> GetOriginalUrlAsync(string shortUrl)
        {
            var urlMapping = await _context.UrlMappings
                .FirstOrDefaultAsync(u => u.ShortUrl == shortUrl);
            return urlMapping?.OriginalUrl;
        }

        public async Task<List<UrlMapping>> GetAllUrlMappingsAsync()
        {
            return await _context.UrlMappings.ToListAsync();
        }

        public async Task DeleteAllUrlMappingsAsync()
        {
            _context.UrlMappings.RemoveRange(_context.UrlMappings);
            await _context.SaveChangesAsync();
        }

        private string GenerateShortUrl()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8); // Short 8-char unique ID
        }
    }
}