using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OAuth.Domain.Model;

namespace OAuth.Data.Mapping
{
    public class SupplierModesMap : EntityTypeConfiguration<SupplierMode>
    {
        public SupplierModesMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("OP_SupplierModes");
            this.Property(t => t.Id).HasColumnName("ID_int");
            this.Property(t => t.SupplierId).HasColumnName("SupplierID_int");
            this.Property(t => t.ModeId).HasColumnName("ModeID_int");

            //关系一对一
            HasRequired(c => c.Mode).WithMany();
        }
    }
}
