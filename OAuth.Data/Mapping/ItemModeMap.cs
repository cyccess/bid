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
    public class ItemModeMap : EntityTypeConfiguration<ItemMode>
    {
        public ItemModeMap()
        {
            ToTable("OP_ItemModes").HasKey(c => c.Id);

            Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Id).HasColumnName("ID_int");
            Property(t => t.ItemID).HasColumnName("ItemID_int");
            Property(t => t.ModeID).HasColumnName("ModeID_int");

            //关系一对一
            HasRequired(p=>p.Mode).WithMany();
        }
    }
}
