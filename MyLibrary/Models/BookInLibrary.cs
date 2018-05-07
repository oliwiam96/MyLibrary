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
        public Book Book { get; set; }

        public int LibraryId { get; set; }
        public Library Library { get; set; }

        public ICollection<Comment> Comments;
        public ICollection<Reading> Readings;
        public ICollection<Rental> Rentals;
    }
}