using System;
using System.Collections.Generic;

namespace PracticeWebApi.Models
{
    public partial class Users
    {
        public Users()
        {
            UserBookAssociation = new HashSet<UserBookAssociation>();
        }

        public int UserId { get; set; }
        public int? RoleId { get; set; }
        public int? LocationId { get; set; }

        public virtual Roles Role { get; set; }
        public virtual ICollection<UserBookAssociation> UserBookAssociation { get; set; }
    }
}
