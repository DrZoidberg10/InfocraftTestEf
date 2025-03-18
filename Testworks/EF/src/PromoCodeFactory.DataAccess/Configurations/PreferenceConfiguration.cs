using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;

namespace PromoCodeFactory.DataAccess.Configurations
{
    public class PreferenceConfiguration : IEntityTypeConfiguration<Preference>
    {
        public void Configure(EntityTypeBuilder<Preference> builder)
        {
            builder.HasKey(p => p.Id);

            builder.
                HasMany(p => p.PromoCodes)
                .WithOne(p => p.Preference)
                .HasForeignKey(p => p.PreferenceId);

            builder.
                HasMany(p => p.Customers)
                .WithMany(c => c.Preferences)
                .UsingEntity<CustomerPreference>();

            builder.
                Property(p => p.Name)
                .HasMaxLength(50);

            builder.HasData(FakeDbData.Preferences);
        }
    }
}
