using OAuth.Domain.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace OAuth.Data.Mapping
{
    public class RoleRightMap : EntityTypeConfiguration<RoleRight>
    {
        public RoleRightMap()
        {
            ToTable("OP_RoleRight")
                .HasKey(c => c.Id);

            Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            //关系一对一，一个角色权限对应的一个菜单
            //HasRequired<Menu>(u => u.Module).WithMany();
        }
    }
}
