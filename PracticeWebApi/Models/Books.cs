using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PracticeWebApi.Models
{
    public partial class Books
    {
        public Books()
        {
            BookLibraryAssociation = new HashSet<BookLibraryAssociation>();
        }

        //[Required]
        public int BookId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        public string Author { get; set; }

        [DataType(DataType.Currency)]
        public float? Price { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Genre { get; set; }

        public virtual ICollection<BookLibraryAssociation> BookLibraryAssociation { get; set; }
    }
}
