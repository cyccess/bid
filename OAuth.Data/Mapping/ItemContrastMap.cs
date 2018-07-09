using OAuth.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Data.Mapping
{
    public class ItemContrastMap : EntityTypeConfiguration<ItemContrast>
    {
        public ItemContrastMap()
        {
            //ToTable("OP_Items").HasKey(c => c.Id);

            //Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //Property(t => t.Id).HasColumnName("ID_int");
        //public string ID { get; set; }
        //public string ItemID { get; set; }
        //public string Mode { get; set; }
        //public string Name { get; set; }
        //public string Spec { get; set; }
        //public string Texture { get; set; }
        //public string Norm { get; set; }
        //public string Extent { get; set; }
        //public string Sum { get; set; }
        //public string TotalWeight { get; set; }
        //public string RealPrice { get; set; }
        //public string ReachTime { get; set; }
        //public string PriceUnit { get; set; }
        //public string PriceMode { get; set; }
        //public string PayMode { get; set; }
        //public string IsTax { get; set; }
        //public string Memo { get; set; }
        //public int SupplierID1 { get; set; }
        //public string SupplierName1 { get; set; }
        //public int SupplierID2 { get; set; }
        //public string SupplierName2 { get; set; }
        //public int SupplierID3 { get; set; }
        //public string SupplierName3 { get; set; }
        //public int SupplierID4 { get; set; }
        //public string SupplierName4 { get; set; }
        //public int SupplierID5 { get; set; }
        //public string SupplierName5 { get; set; }
        //public int SupplierID6 { get; set; }
        //public string SupplierName6 { get; set; }
        //public int SupplierID7 { get; set; }
        //public string SupplierName7 { get; set; }
        //public int SupplierID8 { get; set; }
        //public string SupplierName8 { get; set; }
        //public int InID { get; set; }
        //public string InName { get; set; }
        //public int MinID { get; set; }
        //public string MinName { get; set; }
        //public string Explain { get; set; }
    }
    }
}
