namespace UrlShortener.Models
{
    public class UrlCreateRequest
    {
        public string OriginalUrl { get; set; } = string.Empty;
        public string? CustomAlias { get; set; } // Add this property for custom alias
    }
}