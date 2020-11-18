﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWebApi.Models
{
    public class BookWithStatus
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public float? Price { get; set; }
        public string Genre { get; set; }
        public bool isAvailable { get; set; }
    }
}
