using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLibrary.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public int BookInLibraryId;
        public BookInLibrary BookInLibrary { get; set; }
    }
}