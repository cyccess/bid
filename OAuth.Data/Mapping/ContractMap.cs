using OAuth.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Data.Mapping
{
    public class ContractMap : EntityTypeConfiguration<Contract>
    {
        public ContractMap()
        {
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.TempMode).HasColumnName("TempMode");
            Property(t => t.Texture).HasColumnName("Texture");
            Property(t => t.Spec).HasColumnName("Spec");
            Property(t => t.Extent).HasColumnName("Extent");
            Property(t => t.Norm).HasColumnName("Norm");
            Property(t => t.Amount).HasColumnName("Amount");
            Property(t => t.TotalWeight).HasColumnName("TotalWeight");
            Property(t => t.Price).HasColumnName("Price");
            Property(t => t.TotalPrice).HasColumnName("TotalPrice");
            Property(t => t.PriceUnit).HasColumnName("PriceUnit");
            Property(t => t.Memo).HasColumnName("Memo");
            Property(t => t.Unit).HasColumnName("Unit");
        }
    }
}
