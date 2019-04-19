using BillsPaymentSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillsPaymentSystem.Data.EntityConfiguration
{
    public class BankAccountConfig : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.Property(e => e.BankName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.SwiftCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired();

            builder.HasOne(e => e.PaymentMethod)
                .WithOne(pm => pm.BankAccount);
        }
    }
}
