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
    class DictMap : EntityTypeConfiguration<Dict>
    {
        public DictMap()
        {
            ToTable("OP_Dict").HasKey(c => c.Id);

            Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Id).HasColumnName("DictID_int");
            Property(t => t.DictName).HasColumnName("DictName_nvarchar");
            Property(t => t.DictKey).HasColumnName("DictKey_nvarchar");
            Property(t => t.DictValue).HasColumnName("DictValue_nvarchar");
            Property(t => t.Sort).HasColumnName("Sort_int");
            Property(t => t.Memo).HasColumnName("Memo_nvarchar");

            //关系一对多，一个用户可拥有多个角色
            //HasMany<Item>(u => u.Items).WithRequired();
        }
    }
}
