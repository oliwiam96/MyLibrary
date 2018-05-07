using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyLibrary.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public DateTime StartOfRental { get; set; }
        public DateTime EndOfRental { get; set; }

        //public string UserFromId { get; set; } // niepotrzebne, bo wiadomo, 
        // ze wypozycza wlasciciel biblioteki z ksiazki BookInLibrary
        
        //TODO pomyslec, czy na pewno ok
        
        public string UserToId { get; set; }
        [ForeignKey("UserToId")]
        public virtual ApplicationUser UserTo { get; set; }

        public int BookInLibraryId;
        public BookInLibrary BookInLibrary;
    }
}