using System;
using System.Collections.Generic;

namespace PracticeWebApi.Models
{
    public partial class UserBookAssociation
    {
        public int UserBookAsscId { get; set; }
        public int? UserId { get; set; }
        public int? BookLibraryAsscId { get; set; }
        public DateTime? DueDate { get; set; }

        public virtual BookLibraryAssociation BookLibraryAssc { get; set; }
        public virtual Users User { get; set; }
    }
}
