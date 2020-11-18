using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PracticeWebApi.Models;

namespace PracticeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookLibraryAssociationsController : ControllerBase
    {
        private readonly PracticeDBContext _context;

        public BookLibraryAssociationsController(PracticeDBContext context)
        {
            _context = context;
        }

        // GET: api/BookLibraryAssociations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookLibraryAssociation>>> GetBookLibraryAssociation()
        {
            return await _context.BookLibraryAssociation.ToListAsync();
        }

        // GET: api/BookLibraryAssociations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookLibraryAssociation>> GetBookLibraryAssociation(int id)
        {
            var bookLibraryAssociation = await _context.BookLibraryAssociation.FindAsync(id);

            if (bookLibraryAssociation == null)
            {
                return NotFound();
            }

            return bookLibraryAssociation;
        }

        [HttpGet("checkablebooks")]
        public async Task<ActionResult<List<Books>>> GetAllBooksForCheckout()
        {
            List<int> bookIds = new List<int>();
            foreach (var row in _context.BookLibraryAssociation)
            {
                if (row.IsCheckedOut == false)
                {
                    bookIds.Add((int)row.BookId);
                }
            }
            List<Books> books = new List<Books>();
            foreach (var row in _context.Books)
            {
                if (bookIds.Contains(row.BookId))
                {
                    books.Add(row);
                }
            }

            return books;
        }

        [HttpGet("library/{id}")]
        public async Task<ActionResult<List<Books>>> GetAllBooks(int id) {
            List<int> bookIds = new List<int>();
            List<Books> booksInLibrary = new List<Books>();
            foreach (var bla in _context.BookLibraryAssociation)
            {
                if(bla.LibraryId == id)
                {
                    //booksInLibrary.Add(await _context.Books.FindAsync(id));
                    bookIds.Add((int)bla.BookId);
                }
            }
            foreach(var book in _context.Books)
            {
                if(bookIds.Contains(book.BookId))
                {
                    booksInLibrary.Add(book);
                }
            }
            return booksInLibrary;
        }

        [HttpGet("getAvailability/{id}")]
        public async Task<ActionResult<bool>> GetAvailability(int id)
        {
            var bla = _context.BookLibraryAssociation.FirstOrDefault(i => i.BookId == id);
            return bla.IsAvailable;
        }

            // PUT: api/BookLibraryAssociations/5
            // To protect from overposting attacks, enable the specific properties you want to bind to, for
            // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
            [HttpPut("{id}")]
        public async Task<IActionResult> PutBookLibraryAssociation(int id, [FromBody]BookLibraryAssociation bookLibraryAssociation)
        {
            if (id != bookLibraryAssociation.BookLibraryAsscId)
            {
                return BadRequest();
            }

            _context.Entry(bookLibraryAssociation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookLibraryAssociationExists(id))
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


        [HttpPut("update")]
        public ActionResult<bool> PutBookLibraryAssociation([FromBody] BookAvailability json)
        {
        
            var book = _context.BookLibraryAssociation.FirstOrDefault(x => x.BookId == json.id);

            if (book != null)
            {                
                book.IsAvailable = json.isAvailable;
                _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest(false);
            }
            return Ok(true);
        }
        


        // POST: api/BookLibraryAssociations
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BookLibraryAssociation>> PostBookLibraryAssociation([FromBody]BookLibraryAssociation bookLibraryAssociation)
        {
            _context.BookLibraryAssociation.Add(bookLibraryAssociation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookLibraryAssociation", new { id = bookLibraryAssociation.BookLibraryAsscId }, bookLibraryAssociation);
        }

        public class checkoutBook
        {
            public int bookId { get; set; }
            public int userId { get; set; }
        }

        [HttpPost("checkout")]
        public ActionResult<bool> PostCheckoutBook([FromBody] checkoutBook json)
        {
            var bookLibraryAssc = _context.BookLibraryAssociation.FirstOrDefault(x => x.BookId == json.bookId);

            if (bookLibraryAssc!=null && bookLibraryAssc.IsAvailable==true && bookLibraryAssc.IsCheckedOut == false)
            {
                bookLibraryAssc.IsCheckedOut = true;
                int blaId = bookLibraryAssc.BookLibraryAsscId;
                DateTime dueDate = DateTime.Today.AddMonths(3);
                _context.UserBookAssociation.Add(new UserBookAssociation { UserId = json.userId, BookLibraryAsscId = blaId, DueDate = dueDate });
                _context.SaveChangesAsync();
            } else
            {
                return BadRequest(false);
            }
            return Ok(true);
        }

        public class returnBook
        {
            public int bookId { get; set; }
            //public int userId { get; set; }
        }

        [HttpPost("return")]
        public ActionResult<bool> PostReturnBook([FromBody] returnBook json)
        {
            var bookLibraryAssc = _context.BookLibraryAssociation.FirstOrDefault(x => x.BookId == json.bookId);

            if (bookLibraryAssc!=null && bookLibraryAssc.IsCheckedOut == true)
            {
                bookLibraryAssc.IsCheckedOut = false;
                int blaId = bookLibraryAssc.BookLibraryAsscId;
                //DateTime dueDate = DateTime.Today.AddMonths(3);
                //_context.UserBookAssociation.Add(new UserBookAssociation { UserId = userId, BookLibraryAsscId = blaId, DueDate = dueDate });
                _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest(false);
            }
            return Ok(true);
        }

        
        public class BookID
        {
            public int bookId { get; set; }
        }
        // DELETE: api/BookLibraryAssociations/5
        [HttpDelete]
        public async Task<ActionResult<BookLibraryAssociation>> DeleteBookLibraryAssociation([FromBody] BookID json)
        {
            var bookLibraryAssociation = await _context.BookLibraryAssociation.FindAsync(json.bookId);
            if (bookLibraryAssociation == null)
            {
                return NotFound();
            }

            _context.BookLibraryAssociation.Remove(bookLibraryAssociation);
            await _context.SaveChangesAsync();

            return bookLibraryAssociation;
        }

        private bool BookLibraryAssociationExists(int id)
        {
            return _context.BookLibraryAssociation.Any(e => e.BookLibraryAsscId == id);
        }
    }
}
