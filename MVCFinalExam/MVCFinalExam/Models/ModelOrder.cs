using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCFinalExam.Models
{
    public class ModelOrder
    {
        public int Id_Person { get; set; }
        public int Id_Book { get; set; }
        public int Quantity { get; set; }
        public int InCharge { get; set; }
        public Nullable<int> Id_Seller { get; set; }

        public virtual ModelBook Book { get; set; }
        public virtual ModelPerson Person { get; set; }
        public virtual ModelPerson Person1 { get; set; }
    }
}