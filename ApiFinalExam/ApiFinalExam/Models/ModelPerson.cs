using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiFinalExam.Models
{
    public class ModelPerson
    {
        public int Id { get; set; }
        public int IsSeller { get; set; }
        public string Mail { get; set; }
        public string Pwd { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string CAP { get; set; }
        public string City { get; set; }
    }
}