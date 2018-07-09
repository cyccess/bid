using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Model
{
    public class Contract
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int TempMode { get; set; }

        public string Texture { get; set; }

        public string Spec { get; set; }

        public double Extent { get; set; }

        public string Norm { get; set; }

        public int Amount { get; set; }

        public double TotalWeight { get; set; }

        public double Price { get; set; }

        public double TotalPrice { get; set; }

        public string PriceUnit { get; set; }

        public string Unit { get; set; }

        public string Memo { get; set; }
    }
}
