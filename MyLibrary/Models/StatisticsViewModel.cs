using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLibrary.Models
{
    public class StatisticsViewModel
    {
        public double BooksPerYear { get; set; }
        public Reading LastFinishedReading { get; set;}
        public ICollection<Reading> CurrentReadings { get; set; }
    }
}