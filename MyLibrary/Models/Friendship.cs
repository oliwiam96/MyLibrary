﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyLibrary.Models
{
    public class Friendship
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //[Key, Column(Order = 0)]add
        public string ApplicationUserId { get; set; }
        //[Key, Column(Order = 1)]
        public int LibraryId { get; set; }

        public string Token { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Library Library { get; set; }

        public DateTime StartOfFriendship { get; set; }
    }
}