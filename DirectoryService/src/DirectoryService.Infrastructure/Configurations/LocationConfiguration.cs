using DirectoryService.Domain.Locations;
using DirectoryService.Domain.Locations.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations");

        builder.HasKey(e => e.Id).HasName("pk_location");

        builder.Property(e => e.Id)
            .HasConversion(lId => lId.Value, id => LocationId.Of(id))
            .HasColumnName("id");

        builder.OwnsOne(e => e.Name, nb =>
        {
            nb.Property(n => n.Value)
                .HasColumnName("name")
                .HasMaxLength(LocationName.MAX_LENGHT)
                .IsRequired();

            nb.HasIndex(e => e.Value)
                .IsUnique()
                .HasDatabaseName("ix_locations_name");
        });

        builder.ComplexProperty(e => e.Timezone, tzb =>
        {
            tzb.Property(n => n.Value)
                .HasColumnName("timezone")
                .HasMaxLength(Timezone.MAX_LENGHT)
                .IsRequired();
        });

        builder.OwnsOne(e => e.Address, ab =>
        {
            ab.ToJson("address");

            ab.Property(a => a.Country)
                .HasColumnName("country")
                .HasMaxLength(Address.MAX_LENGTH_COUNTRY)
                .IsRequired();

            ab.Property(a => a.PostalCode)
                .HasColumnName("postal_code")
                .HasMaxLength(Address.LENGTH_POSTAL_CODE)
                .IsRequired();

            ab.Property(a => a.Region)
                .HasColumnName("region")
                .HasMaxLength(Address.MAX_LENGTH_REGION)
                .IsRequired();

            ab.Property(a => a.City)
                .HasColumnName("city")
                .HasMaxLength(Address.MAX_LENGTH_CITY)
                .IsRequired();

            ab.Property(a => a.Street)
                .HasColumnName("street")
                .HasMaxLength(Address.MAX_LENGTH_STREET)
                .IsRequired();

            ab.Property(a => a.HouseNumber)
                .HasColumnName("house_number")
                .HasMaxLength(Address.MAX_LENGTH_HOUSE_NUMBER)
                .IsRequired();
        });

        builder.Property(e => e.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();
    }
}