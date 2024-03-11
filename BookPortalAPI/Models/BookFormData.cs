using Microsoft.AspNetCore.Http;

namespace BookPortalAPI.Models
{
    public class BookFormData
    {
        public Book? Book { get; set; }
        public IFormFile? CoverImage { get; set; }
    }
}
