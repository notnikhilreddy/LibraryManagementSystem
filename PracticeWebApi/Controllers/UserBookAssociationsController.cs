using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PracticeWebApi.Models;

namespace PracticeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBookAssociationsController : ControllerBase
    {
        private readonly PracticeDBContext _context;

        public UserBookAssociationsController(PracticeDBContext context)
        {
            _context = context;
        }

        // GET: api/UserBookAssociations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserBookAssociation>>> GetUserBookAssociation()
        {
            return await _context.UserBookAssociation.ToListAsync();
        }

        // GET: api/UserBookAssociations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserBookAssociation>> GetUserBookAssociation(int id)
        {
            var userBookAssociation = await _context.UserBookAssociation.FindAsync(id);

            if (userBookAssociation == null)
            {
                return NotFound();
            }

            return userBookAssociation;
        }


        public class UserId
        {
            public int userId { get; set; }
        }
        [HttpGet("mycheckouts/{id}")]
        public async Task<ActionResult<List<BookWithDuedate>>> GetMyCheckouts(int id)
        {
            List<BookWithDuedate> bwdd = new List<BookWithDuedate>();
            List<int> blaIds = new List<int>();

            foreach (var row in _context.UserBookAssociation) {
                if(row.UserId == id)
                {
                    blaIds.Add((int)row.BookLibraryAsscId);
                    
                }
            }

            List<int> bookIds = new List<int>();
            foreach (var row in _context.BookLibraryAssociation)
            {
                if (blaIds.Contains(row.BookLibraryAsscId))
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

            foreach (Books book in books)
            {
                int blaId = _context.BookLibraryAssociation.FirstOrDefault(i => i.BookId == book.BookId).BookLibraryAsscId;
                DateTime due = (DateTime)_context.UserBookAssociation.FirstOrDefault(i => i.BookLibraryAsscId == blaId).DueDate;
                bwdd.Add(new BookWithDuedate { BookId = book.BookId, Title = book.Title, Author = book.Author, Price = book.Price, Genre = book.Genre, DueDate = due});
            }
            List<BookWithDuedate> sortedList = bwdd.OrderBy(p => p.DueDate).ToList();

            return Ok(bwdd);
        }

        // PUT: api/UserBookAssociations/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserBookAssociation(int id, [FromBody]UserBookAssociation userBookAssociation)
        {
            if (id != userBookAssociation.UserBookAsscId)
            {
                return BadRequest();
            }

            _context.Entry(userBookAssociation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserBookAssociationExists(id))
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

        // POST: api/UserBookAssociations
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UserBookAssociation>> PostUserBookAssociation([FromBody]UserBookAssociation userBookAssociation)
        {
            _context.UserBookAssociation.Add(userBookAssociation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserBookAssociation", new { id = userBookAssociation.UserBookAsscId }, userBookAssociation);
        }


        public class BookId
        {
            public int id { get; set; }
        }
        // DELETE: api/UserBookAssociations/5
        [HttpDelete("delete")]
        public async Task<ActionResult<UserBookAssociation>> DeleteUserBookAssociation([FromBody] BookId json)
        {
            var userBookAssociation = await _context.UserBookAssociation.FindAsync(json.id);
            if (userBookAssociation == null)
            {
                return NotFound();
            }

            _context.UserBookAssociation.Remove(userBookAssociation);
            await _context.SaveChangesAsync();

            return userBookAssociation;
        }



        private bool UserBookAssociationExists(int id)
        {
            return _context.UserBookAssociation.Any(e => e.UserBookAsscId == id);
        }
    }
}
