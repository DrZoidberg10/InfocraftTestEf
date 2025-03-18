using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;

namespace PromoCodeFactory.DataAccess.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);

            builder.
                HasMany(r => r.Employees)
                .WithOne(e => e.Role)
                .HasForeignKey(e => e.RoleId);

            builder.
                Property(r => r.Name)
                .HasMaxLength(50);

            builder.
                Property(r => r.Description)
                .HasMaxLength(50);

            builder.HasData(FakeDbData.Roles);
        }
    }
}
