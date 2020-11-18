using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeWebApi.Models;

namespace PracticeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly PracticeDBContext _context;

        public BooksController(PracticeDBContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Books>>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Books>> GetBooks(int id)
        {
            var books = await _context.Books.FindAsync(id);

            if (books == null)
            {
                return NotFound();
            }

            return books;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.

      
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooks(int id, [FromBody]Books book)
        {
            if (id != book.BookId)
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
                if (!BooksExists(id))
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

        // POST: api/Books
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Books>> PostBooks([FromBody]Books books)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(books);
                await _context.SaveChangesAsync();

                _context.BookLibraryAssociation.Add(new BookLibraryAssociation { BookId = books.BookId, LibraryId = 1, IsAvailable = true, IsCheckedOut = false});
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetBooks", new { id = books.BookId }, books);
            } else
            {
                return BadRequest(400);
            }
        }

        public class BookId
        {
            public int id { get; set; }
        }
        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Books>> DeleteBooks([FromBody]BookId json)
        {
            var books = await _context.Books.FindAsync(json.id);
            if (books == null)
            {
                return NotFound();
            }

            _context.Books.Remove(books);
            await _context.SaveChangesAsync();

            return books;
        }

        private bool BooksExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}
