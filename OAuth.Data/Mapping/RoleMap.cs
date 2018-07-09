using OAuth.Domain.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace OAuth.Data.Mapping
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            ToTable("OP_Roles")
                  .HasKey(c => c.Id);

            Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Id).HasColumnName("RoleID_int");
            Property(t => t.Name).HasColumnName("Role_nvarchar");
            Property(t => t.AddDate).HasColumnName("AddTime_datetime");
            Property(t => t.Status).HasColumnName("Status_tinyint");

            //关系一对多，一个角色对应的多个角色权限
            HasMany<RoleRight>(u => u.RoleRights).WithRequired();
        }
    }
}