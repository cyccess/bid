using OAuth.Domain.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace OAuth.Data.Mapping
{
    public class UserRoleMap : EntityTypeConfiguration<UserRole>
    {
        public UserRoleMap()
        {
            ToTable("OP_UserRole")
                .HasKey(c => c.Id);

            Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.UserId).HasColumnName("UserID_int");
            Property(c => c.RoleId).HasColumnName("RoleID_int");
            Property(c => c.Status).HasColumnName("Status_tinyint");

            //关系一对一 用户拥有角色对应的角色信息
            HasRequired(c => c.Role).WithMany();
        }
    }
}
