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
    public class ItemMap : EntityTypeConfiguration<Item>
    {
        public ItemMap()
        {
            ToTable("OP_Items").HasKey(c => c.Id);

            Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Id).HasColumnName("ID_int");
            Property(t => t.ItemName).HasColumnName("ItemName_nvarchar");
            Property(t => t.StartDate).HasColumnName("StartDate_datetime");
            Property(t => t.EndDate).HasColumnName("EndDate_datetime");
            Property(t => t.ItemNo).HasColumnName("ItemNo_nvarchar");
            Property(t => t.TempMode).HasColumnName("TempMode_int");
            Property(t => t.InputPerson).HasColumnName("InputPerson_int");
            Property(t => t.InputTime).HasColumnName("InputTime_datetime");
            Property(t => t.Memo).HasColumnName("Memo_nvarchar");
            Property(t => t.IsNotice).HasColumnName("IsNotice_bit");
            Property(t => t.SignTime).HasColumnName("SignTime_datetime");
            Property(t => t.SignPlace).HasColumnName("SignPlace_nvarchar");
            Property(t => t.IsUpdate).HasColumnName("IsUpdate_bit");
            //关系一对多，一个用户可拥有多个角色
            HasMany<ItemMode>(u => u.ItemModes).WithRequired();
            HasRequired(c => c.User).WithMany().HasForeignKey(p => p.InputPerson);
        }
    }
}
