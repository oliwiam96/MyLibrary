using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLibrary.Models
{
    public class BookInLibrary
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        public int LibraryId { get; set; }
        public virtual Library Library { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Reading> Readings { get; set; }
        public virtual ICollection<Rental> Rentals { get; set; }
    }
}