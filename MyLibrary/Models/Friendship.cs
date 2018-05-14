using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyLibrary.Models
{
    public class Friendship
    {
        [Key, Column(Order = 0)]
        public string ApplicationUserId { get; set; }
        [Key, Column(Order = 1)]
        public int LibraryId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Library Library { get; set; }

        public DateTime StartOfFriendship { get; set; }
    }
}