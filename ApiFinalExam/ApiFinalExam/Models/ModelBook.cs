using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiFinalExam.Models
{
    public class ModelBook
    {
        public int ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}