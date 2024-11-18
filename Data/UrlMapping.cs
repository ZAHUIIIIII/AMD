using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Data
{
    public class UrlMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string OriginalUrl { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "text")]
        public string ShortUrl { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "timestamptz")] // Use timestamp with time zone
        public DateTime CreatedAt { get; set; }
    }
}