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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Key, ForeignKey("User")]
        public int Id { get; set; }

        /*public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }*/

        public virtual ApplicationUser ApplicationUser { get; set; }

        public ICollection<BookInLibrary> BooksInLibrary { get; set; }
        public ICollection<Friendship> ApplicationUsers { get; set; } // przyjaciele, ktorych User zaprosil do swojej biblioteki

    }
}