using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyLibrary.Models
{
    public class RentalExternal
    {
        public int Id { get; set; }
        public string Status { get; set; } // TODO enum???
        public DateTime StartOfRental { get; set; }
        public DateTime EndOfRental { get; set; }
        public string LenderName { get; set; } // e.g. a name of a person who doesn't have an account in the system

        public string UserToId { get; set; }
        [ForeignKey("UserToId")]
        public virtual ApplicationUser UserTo { get; set; }


        public int BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}