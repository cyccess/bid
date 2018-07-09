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
    public class ItemSureMap : EntityTypeConfiguration<ItemSure>
    {
        public ItemSureMap()
        {
            ToTable("OP_ItemSure").HasKey(c => c.Id);

            Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Id).HasColumnName("ID_int");
            Property(t => t.MaterialID).HasColumnName("MaterialID_int");
            Property(t => t.QuoteID).HasColumnName("QuoteID_int");
            Property(t => t.InputPerson).HasColumnName("InputPerson_int");
            Property(t => t.InputTime).HasColumnName("InputTime_datetime");
            Property(t => t.Memo).HasColumnName("Memo_nvarchar");
        }
    }
}
