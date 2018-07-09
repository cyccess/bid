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
    public class ModeMap : EntityTypeConfiguration<Mode>
    {
        public ModeMap()
        {
            ToTable("OP_Modes")
                .HasKey(c => c.Id);

            Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Id).HasColumnName("ModeID_int");
            Property(t => t.ModeKey).HasColumnName("ModeKey_nvarchar");
            Property(t => t.ModeName).HasColumnName("ModeName_nvarchar");
            Property(t => t.ParentID).HasColumnName("ParentID_int");
            Property(t => t.IsEnabled).HasColumnName("IsEnabled_bit");
        }
    }
}
