using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyLibrary.Models
{
    public class BookInLibraryViewModel
    {
        public BookInLibrary BookInLibrary { get; set; }

        public Boolean IsLent { get; set; }
        public String BorrowerName { get; set; }

        public Boolean IsCurrentlyReadBySbElse { get; set; }
        public String OtherReaderName { get; set; } //set if is currently read by sb else
        public Boolean IsCurrentlyReadByMe { get; set; }
        public SelectList Users { get; set; }

        public string Comment { get; set; }
        public string UserId { get; set; }
    }
}