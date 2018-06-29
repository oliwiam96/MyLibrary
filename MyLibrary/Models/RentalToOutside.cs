using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLibrary.Models
{
    public class RentalToOutside
    {
        public int Id { get; set; }
        public DateTime StartOfRental { get; set; }
        public DateTime? EndOfRental { get; set; }

        public int BookInLibraryId { get; set; }
        public virtual BookInLibrary BookInLibrary { get; set; }
    }
}