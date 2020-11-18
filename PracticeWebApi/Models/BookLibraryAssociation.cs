using System;
using System.Collections.Generic;

namespace PracticeWebApi.Models
{
    public partial class BookLibraryAssociation
    {
        public BookLibraryAssociation()
        {
            UserBookAssociation = new HashSet<UserBookAssociation>();
        }

        public int BookLibraryAsscId { get; set; }
        public int? BookId { get; set; }
        public int? LibraryId { get; set; }
        public bool? IsAvailable { get; set; }
        public bool? IsCheckedOut { get; set; }

        public virtual Books Book { get; set; }
        public virtual Library Library { get; set; }
        public virtual ICollection<UserBookAssociation> UserBookAssociation { get; set; }
    }
}
