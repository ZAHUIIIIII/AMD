using UrlShortener.Data;

namespace UrlShortener.Services
{
    public interface IUrlService
    {
        Task<string> ShortenUrlAsync(string originalUrl, string? customAlias = null);
        Task<string?> GetOriginalUrlAsync(string shortUrl);
        Task<List<UrlMapping>> GetAllUrlMappingsAsync();
    }
}