
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookPortalAPI.Data;
using BookPortalAPI.Models;


namespace BookPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Book
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

        // GET: api/Book/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Book/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Book
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook([FromBody] Book book)
        {
            try
            {
                if (book == null)
                {
                    return BadRequest("Ingen giltig data mottagen.");
                }

                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetBook", new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                // Logga felmeddelandet eller hantera det på annat sätt
                return StatusCode(500, $"Ett fel uppstod vid skapande av boken: {ex.Message}");
            }
        }
        // DELETE: api/Book/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

        // GET: api/Book/GetBooksByUserId
        [HttpGet("GetBooksByUserId")]
        public async Task<IActionResult> GetBooks(int? userId = null)
        {
            IQueryable<Book> booksQuery = _context.Books;

            // Filtrera böcker baserat på användarens ID om det finns
            if (userId.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.Reviews != null && b.Reviews.Any(r => r.UserId == userId));
            }

            var books = await booksQuery.ToListAsync();
            return Ok(books);
        }

        // POST: api/Book/AddReview/{id}
        [HttpPost("AddReview/{id}")]
        public async Task<IActionResult> AddReview(int id, Review review)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return NotFound();
                }

                // Tilldela recensionen till rätt bok
                review.BookId = id;

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetReview", new { id = review.Id }, review);
            }
            catch (Exception ex)
            {
                // Logga felmeddelandet eller hantera det på annat sätt
                return StatusCode(500, $"Ett fel uppstod vid tillägg av recensionen: {ex.Message}");
            }
        }

        // GET: api/Book/GetReviews/{id}
        [HttpGet("GetReviews/{id}")]
        public async Task<IActionResult> GetReviews(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            // Hämta recensioner för den angivna boken
            var reviews = await _context.Reviews.Where(r => r.BookId == id).ToListAsync();
            return Ok(reviews);
        }
        // POST: api/Book/AddRating/{id}
        [HttpPost("AddRating/{id}")]
        public async Task<IActionResult> AddRating(int id, int rating)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return NotFound();
                }

                // Uppdatera betyget för den angivna boken
                book.Rating = rating;

                _context.Entry(book).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                // Logga felmeddelandet eller hantera det på annat sätt
                return StatusCode(500, $"Ett fel uppstod vid tillägg av betyget: {ex.Message}");
            }
        }

        // GET: api/Book/GetRating/{id}
        [HttpGet("GetRating/{id}")]
        public async Task<IActionResult> GetRating(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            // Hämta betyget för den angivna boken
            var rating = book.Rating;
            return Ok(rating);
        }


    }
}
