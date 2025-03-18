using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System.Reflection.Emit;
using PromoCodeFactory.DataAccess.Data;

namespace PromoCodeFactory.DataAccess.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.
                HasMany(c => c.PromoCodes)
                .WithOne(p => p.Customer)
                .HasForeignKey(p => p.CustomerId);

            builder.
                HasMany(c => c.Preferences)
                .WithMany(p => p.Customers)
                .UsingEntity<CustomerPreference>();

            builder.
                Property(c => c.FirstName)
                .HasMaxLength(50);

            builder.
                Property(c => c.LastName)
                .HasMaxLength(50);

            builder.
                Property(c => c.Email)
                .HasMaxLength(50);

            builder.HasData(FakeDbData.Customers);
        }
    }
}
