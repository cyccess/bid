using OAuth.Domain.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace OAuth.Data.Mapping
{
    public class ModulePermissionMap : EntityTypeConfiguration<ModulePermission>
    {
        public ModulePermissionMap()
        {
            ToTable("OP_ModulePermission")
                .HasKey(c => c.Id);

            Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //关系一对一 ，对应一个模块
            HasRequired(c => c.Permission).WithMany();

        }
    }
}
