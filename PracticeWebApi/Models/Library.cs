using System;
using System.Collections.Generic;

namespace PracticeWebApi.Models
{
    public partial class Library
    {
        public Library()
        {
            BookLibraryAssociation = new HashSet<BookLibraryAssociation>();
        }

        public int LibraryId { get; set; }
        public int? LocationId { get; set; }

        public virtual ICollection<BookLibraryAssociation> BookLibraryAssociation { get; set; }
    }
}
