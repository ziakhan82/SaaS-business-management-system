using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceFlow.Domain.Entities;

namespace ServiceFlow.Infrastructure.Persistence.Configurations;

public sealed class CustomerConfiguration
    : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(customer => customer.Id);

        builder.Property(customer => customer.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(customer => customer.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(customer => customer.Email)
            .IsRequired()
            .HasMaxLength(320);

        builder.Property(customer => customer.Phone)
            .HasMaxLength(50);

        builder.Property(customer => customer.Address)
            .HasMaxLength(250);

        builder.Property(customer => customer.PostalCode)
            .HasMaxLength(20);

        builder.Property(customer => customer.City)
            .HasMaxLength(100);

        builder.Property(customer => customer.CreatedAtUtc)
            .IsRequired();

        builder.HasIndex(customer => customer.CompanyId);

        builder.HasIndex(customer => new
        {
            customer.CompanyId,
            customer.Email
        })
        .IsUnique();
    }
}