using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyLibrary.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required!")]
        [StringLength(100, ErrorMessage = "Maximal length of the title of a book is 100 characters!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required!")]
        [StringLength(100, ErrorMessage = "Maximal length of the author of a book is 100 characters!")]
        public string Author { get; set; }

        [Display(Name = "Link to a photo of a cover (url)")]
        public string UrlPhoto { get; set; }

        public ICollection<BookInLibrary> BooksInLibrary { get; set; }
    }
}