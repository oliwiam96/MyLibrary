using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyLibrary.Models
{
    public class Library
    {
        [Key, ForeignKey("User")]
        public string Id { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public ICollection<BookInLibrary> BooksInLibrary;

        //public ICollection<ApplicationUser> ApplicationUsers; // przyjaciele, ktorych user zaprosil do swojej biblioteki
    }
}