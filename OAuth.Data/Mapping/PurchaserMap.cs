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
    public class PurchaserMap : EntityTypeConfiguration<Purchaser>
    {
        public PurchaserMap(){
            this.HasKey(t => t.Id);

            this.Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("OP_Purchasers");
            this.Property(t => t.Id).HasColumnName("PurchaserID_int");
            this.Property(t => t.PurchaserName).HasColumnName("PurchaserName_nvarchar");
            this.Property(t => t.Address).HasColumnName("Address_nvarchar");
            this.Property(t => t.LegalPerson).HasColumnName("LegalPerson_nvarchar");
            this.Property(t => t.EntrustPerson).HasColumnName("EntrustPerson_nvarchar");
            this.Property(t => t.Fax).HasColumnName("Fax_nvarchar");
            this.Property(t => t.BankName).HasColumnName("BankName_nvarchar");
            this.Property(t => t.BankNo).HasColumnName("BankNo_nvarchar");
        }
    }
}
