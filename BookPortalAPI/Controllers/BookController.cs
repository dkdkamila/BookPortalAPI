
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
        public async Task<ActionResult<Book>> PostBook([FromForm] BookFormData formData)
        {
            try
            {
                if (formData == null || formData.Book == null)
                {
                    return BadRequest("Ingen giltig data mottagen.");
                }

                if (formData.CoverImage != null && formData.CoverImage.Length > 0)
                {

                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await formData.CoverImage.CopyToAsync(memoryStream);
                            formData.Book.CoverImage = memoryStream.ToArray();
                        }
                    }
                }

                _context.Books.Add(formData.Book);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetBook", new { id = formData.Book.Id }, formData.Book);
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

    }
}
