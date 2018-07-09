using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Model
{
    public class ItemQuote : AggregateRoot
    {

        public int ItemMaterialId { get; set; }

        public double Price { get; set; }

        public string Memo { get; set; }

        public string Term { get; set; }


        public int SupplierId { get; set; }

        public DateTime InputTime { get; set; }


    }
}
