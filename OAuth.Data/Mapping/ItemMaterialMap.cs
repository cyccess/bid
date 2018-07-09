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
    public class ItemMaterialMap : EntityTypeConfiguration<ItemMaterial>
    {
        public ItemMaterialMap()
        {
            ToTable("OP_ItemMaterials").HasKey(c => c.Id);
            this.Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Id).HasColumnName("ID_int");
            this.Property(t => t.ItemID).HasColumnName("ItemID_int");
            this.Property(t => t.Mode).HasColumnName("Mode_nvarchar");
            this.Property(t => t.Name).HasColumnName("Name_nvarchar");
            this.Property(t => t.Spec).HasColumnName("Spec_nvarchar");
            this.Property(t => t.Texture).HasColumnName("Texture_nvarchar");
            this.Property(t => t.Norm).HasColumnName("Norm_nvarchar");
            this.Property(t => t.Extent).HasColumnName("Extent_float");
            this.Property(t => t.Sum).HasColumnName("Sum_int");
            this.Property(t => t.TotalWeight).HasColumnName("TotalWeight_float");
            this.Property(t => t.RealPrice).HasColumnName("RealPrice_float");
            this.Property(t => t.ReachTime).HasColumnName("ReachTime_nvarchar");
            this.Property(t => t.PriceUnit).HasColumnName("PriceUnit_nvarchar");
            this.Property(t => t.PriceMode).HasColumnName("PriceMode_nvarchar");
            this.Property(t => t.PayMode).HasColumnName("PayMode_nvarchar");
            this.Property(t => t.IsTax).HasColumnName("IsTax_nvarchar");
            this.Property(t => t.Memo).HasColumnName("Memo_nvarchar");
            this.Property(t => t.UsePlace).HasColumnName("UsePlace_nvarchar");
            this.Property(t => t.IsEnabled).HasColumnName("IsEnabled_bit");
            this.Property(t => t.Term).HasColumnName("Term_nvarchar");

            Ignore(d => d.ItemSure);

            //关系一对多，一个型材对应多个报价
            HasMany(c => c.ItemQuotes).WithRequired();
        }
    }
}
