using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyLibrary.Models
{
    public class OthersViewModel
    {
        public SelectList Libraries { get; set; }
        public string LibraryId { get; set; }
    }
}