using DirectoryService.Domain.DepartmentLocations;
using DirectoryService.Domain.DepartmentLocations.ValueObjects;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configurations;

public class DepartmentLocationConfiguration : IEntityTypeConfiguration<DepartmentLocation>
{
    public void Configure(EntityTypeBuilder<DepartmentLocation> builder)
    {
        builder.ToTable("department_locations");

        builder.HasKey(e => e.Id)
            .HasName("pk_department_location");

        builder.Property(e => e.Id)
            .HasConversion(dId => dId.Value, id => DepartmentLocationId.Of(id))
            .HasColumnName("id");

        builder.HasOne<Department>().WithMany(e => e.Locations)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(e => e.DepartmentId).HasColumnName("department_id");

        builder.HasOne<Location>().WithMany()
            .HasForeignKey(e => e.LocationId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(e => e.LocationId).HasColumnName("location_id");
    }
}