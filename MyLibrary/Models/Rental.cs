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
        public string Status { get; set; }
        public DateTime StartOfRental { get; set; }
        public DateTime EndOfRental { get; set; }

        //public string UserFromId { get; set; } // niepotrzebne, bo wiadomo, 
        // ze wypozycza wlasciciel biblioteki z ksiazki BookInLibrary
        
        
        public string UserToId { get; set; }
        [ForeignKey("UserToId")]
        public virtual ApplicationUser UserTo { get; set; }

        public int BookInLibraryId { get; set; }
        public virtual BookInLibrary BookInLibrary { get; set; }
    }
}