using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLibrary.Models
{
    public class LibraryViewModel
    {
        // TODO is virtual needed?
        public Library Library { get; set; } // myLibrary

        public ICollection<BookInLibrary> BookInLibraryCurrent { get; set; }
        public ICollection<BookInLibrary> BookInLibraryRentalOutside { get; set; }
        public ICollection<BookInLibrary> BookInLibraryRentalInside { get; set; }
        

    }
}