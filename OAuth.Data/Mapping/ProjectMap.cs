using OAuth.Domain.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace OAuth.Data.Mapping
{
    public class ProjectMap : EntityTypeConfiguration<Project>
    {
        public ProjectMap()
        {
            ToTable("OP_Projects")
                .HasKey(c => c.Id);

           
            Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            //关系一对多，一个项目可拥有多个角色
            HasMany<Role>(u => u.Roles).WithRequired();

        }
    }
}
