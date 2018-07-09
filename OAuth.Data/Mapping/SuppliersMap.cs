using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OAuth.Domain.Model;

namespace OAuth.Data.Mapping
{
    public class SuppliersMap : EntityTypeConfiguration<Supplier>
    {
        public SuppliersMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            this.Property(c => c.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Properties
            this.Property(t => t.UserName)
            .HasMaxLength(50);

            this.Property(t => t.Password)
            .IsRequired()
            .HasMaxLength(100);

            this.Property(t => t.SupplierName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.HandleName)
                .HasMaxLength(50);

            this.Property(t => t.MobilePhone)
                .HasMaxLength(15);

            this.Property(t => t.FixedPhone)
                .HasMaxLength(15);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Address)
                .HasMaxLength(150);

            this.Property(t => t.Postcode)
                .HasMaxLength(10);

            this.Property(t => t.Fax)
                .HasMaxLength(15);

            this.Property(t => t.BankName)
                .HasMaxLength(50);

            this.Property(t => t.BankNo)
                .HasMaxLength(50);

            this.Property(t => t.TaxNo)
                .HasMaxLength(50);

            this.Property(t => t.Memo)
                .HasMaxLength(500);

            this.Property(t => t.LegalPerson)
                .HasMaxLength(150);

            this.Property(t => t.EntrustPerson)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("OP_Suppliers");
            this.Property(t => t.Id).HasColumnName("SupplierID_int");
            this.Property(t => t.UserName).HasColumnName("UserName_nvarchar");
            this.Property(t => t.Password).HasColumnName("Password_nvarchar");
            this.Property(t => t.SupplierName).HasColumnName("SupplierName_nvarchar");
            this.Property(t => t.Status).HasColumnName("Status_int");
            this.Property(t => t.HandleName).HasColumnName("HandleName_nvarchar");
            this.Property(t => t.HandleTime).HasColumnName("HandleTime_datetime");
            this.Property(t => t.MobilePhone).HasColumnName("MobilePhone_nvarchar");
            this.Property(t => t.FixedPhone).HasColumnName("FixedPhone_nvarchar");
            this.Property(t => t.Email).HasColumnName("Email_nvarchar");
            this.Property(t => t.Address).HasColumnName("Address_nvarchar");
            this.Property(t => t.Postcode).HasColumnName("Postcode_nvarchar");
            this.Property(t => t.Fax).HasColumnName("Fax_nvarchar");
            this.Property(t => t.BankName).HasColumnName("BankName_nvarchar");
            this.Property(t => t.BankNo).HasColumnName("BankNo_nvarchar");
            this.Property(t => t.TaxNo).HasColumnName("TaxNo_nvarchar");
            this.Property(t => t.PayTime).HasColumnName("PayTime_int");
            this.Property(t => t.Memo).HasColumnName("Memo_nvarchar");
            this.Property(t => t.IsActivated).HasColumnName("IsActivated_bit");
            this.Property(t => t.NoticeTime).HasColumnName("NoticeTime_datetime");
            this.Property(t => t.LegalPerson).HasColumnName("LegalPerson_nvarchar");
            this.Property(t => t.EntrustPerson).HasColumnName("EntrustPerson_nvarchar");
            this.Property(t => t.IsEnabled).HasColumnName("IsEnabled_bit");


            //关系一对多，一个角色对应的多个角色权限
            HasMany<SupplierMode>(u => u.SupplierModes).WithRequired();
        }
    }
}
