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

        public async Task<string> ShortenUrlAsync(string originalUrl)
        {
            var shortUrl = GenerateShortUrl();
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

        private string GenerateShortUrl()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8); // Short 8-char unique ID
        }
    }
}