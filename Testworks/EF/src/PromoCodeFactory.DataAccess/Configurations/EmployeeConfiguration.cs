using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;

namespace PromoCodeFactory.DataAccess.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            builder.
                HasOne(e => e.Role)
                .WithMany(r => r.Employees);

            builder.
                Property(e => e.FirstName)
                .HasMaxLength(50);

            builder.
                Property(e => e.LastName)
                .HasMaxLength(50);

            builder.
                Property(e => e.Email)
                .HasMaxLength(50);

            builder.HasData(FakeDbData.Employees);
        }
    }
}
