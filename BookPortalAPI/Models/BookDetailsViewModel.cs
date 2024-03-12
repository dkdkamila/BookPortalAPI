using System.Collections.Generic;

namespace BookPortalAPI.Models
{
    public class BookDetailsViewModel
    {
        public Book? Book { get; set; }
        public List<Review>? Reviews { get; set; }
    }
}
