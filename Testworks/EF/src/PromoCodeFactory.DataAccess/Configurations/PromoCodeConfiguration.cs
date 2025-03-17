using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;

namespace PromoCodeFactory.DataAccess.Configurations
{
    public class PromoCodeConfiguration : IEntityTypeConfiguration<PromoCode>
    {
        public void Configure(EntityTypeBuilder<PromoCode> builder)
        {
            builder.HasKey(p => p.Id);

            builder.
                HasOne(p => p.Customer)
                .WithMany(c => c.PromoCodes);

            builder.
                HasOne(p => p.Preference)
                .WithMany(p => p.PromoCodes);

            builder.
                Property(p => p.Code)
                .HasMaxLength(50);

            builder.
                Property(c => c.ServiceInfo)
                .HasMaxLength(50);

            builder.
                Property(c => c.PartnerName)
                .HasMaxLength(50);

            builder.
                HasData(FakeDbData.PromoCodes);
        }
    }
}
