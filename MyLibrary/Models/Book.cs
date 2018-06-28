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

        [Display(Name = "Tytuł")]
        [Required(ErrorMessage = "Tytuł jest wymagany!")]
        [StringLength(100, ErrorMessage = "Maksymalna liczba znaków dla tytułu to 100!")]
        public string Title { get; set; }

        [Display(Name = "Autor")]
        [Required(ErrorMessage = "Autor jest wymagany!")]
        [StringLength(100, ErrorMessage = "Maksymalna liczba znaków dla autora to 100!")]
        public string Author { get; set; }

        [Display(Name = "Link do zdjęcia okładki")]
        public string UrlPhoto { get; set; }

        public virtual ICollection<BookInLibrary> BooksInLibrary { get; set; }
    }
}