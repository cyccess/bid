using OAuth.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Service.ModelDto
{
    public class ItemMaterialDto
    {
        public int Id { get; set; }
        public int ItemID { get; set; }
        public string Mode { get; set; }
        public string Name { get; set; }
        public string Spec { get; set; }
        public string Texture { get; set; }
        public string Norm { get; set; }
        public double Extent { get; set; }
        public int Sum { get; set; }
        public double TotalWeight { get; set; }
        public double RealPrice { get; set; }
        public string ReachTime { get; set; }
        public string PriceUnit { get; set; }
        public string PriceMode { get; set; }
        public string PayMode { get; set; }
        public string IsTax { get; set; }
        public string Memo { get; set; }

        public string UsePlace { get; set; }

        public bool IsEnabled { get; set; }

        public string Term { get; set; }


    }
}
