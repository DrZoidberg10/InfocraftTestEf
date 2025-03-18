using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;

namespace PromoCodeFactory.DataAccess.Configurations
{
    public class CustomerPreferenceConfiguration : IEntityTypeConfiguration<CustomerPreference>
    { 

        public void Configure(EntityTypeBuilder<CustomerPreference> builder)
        {
            builder.
                HasKey(cp => new { cp.CustomerId, cp.PreferenceId });

            builder.
                HasData(FakeDbData.CustomerPreferences);
        }
    }
}
