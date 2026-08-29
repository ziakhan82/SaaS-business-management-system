using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceFlow.Domain.Entities;

namespace ServiceFlow.Infrastructure.Persistence.Configurations;

public sealed class CompanyConfiguration
    : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.HasKey(company => company.Id);

        builder.Property(company => company.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(company => company.Email)
            .IsRequired()
            .HasMaxLength(320);

        builder.Property(company => company.Phone)
            .HasMaxLength(50);

        builder.Property(company => company.CreatedAtUtc)
            .IsRequired();

        builder.HasMany(company => company.Customers)
            .WithOne(customer => customer.Company)
            .HasForeignKey(customer => customer.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}