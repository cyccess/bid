using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuth.Domain.Model;

namespace OAuth.Data.Mapping
{
    public class ItemQuoteMap : EntityTypeConfiguration<ItemQuote>
    {
        public ItemQuoteMap()
        {
            ToTable("OP_ItemQuote")
                .HasKey(c => c.Id);



            Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Id).HasColumnName("ID_int");
            Property(t => t.ItemMaterialId).HasColumnName("MaterialID_int");
            Property(t => t.Price).HasColumnName("Price_float");
            Property(t => t.Memo).HasColumnName("Memo_nvarchar");
            Property(t => t.Term).HasColumnName("Term_nvarchar");
            Property(t => t.SupplierId).HasColumnName("SupplierID_int");
            Property(t => t.InputTime).HasColumnName("InputTime_datetime");
            
        }
    }
}
