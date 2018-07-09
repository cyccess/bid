using OAuth.Domain.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;


namespace OAuth.Data.Mapping
{
   public class UserProjectMap : EntityTypeConfiguration<UserProject>
    {
       public UserProjectMap()
        {
            ToTable("OP_UserProject")
                .HasKey(c => c.Id);

            Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //关系一对一，用户项目对应项目
            HasRequired(c => c.Project).WithMany();
        }
    }
}
