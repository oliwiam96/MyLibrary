using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyLibrary.Models
{
    public class Reading
    {
        public int Id { get; set; }
        public DateTime StartOfReading { get; set; }
        public DateTime EndOfReading { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public int BookInLibraryId;
        public BookInLibrary BookInLibrary;
    }
}