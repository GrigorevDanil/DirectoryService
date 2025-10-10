using DirectoryService.Domain.DepartmentPositions;
using DirectoryService.Domain.DepartmentPositions.ValueObjects;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Positions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configurations;

public class DepartmentPositionConfiguration : IEntityTypeConfiguration<DepartmentPosition>
{
    public void Configure(EntityTypeBuilder<DepartmentPosition> builder)
    {
        builder.ToTable("department_positions");

        builder.HasKey(e => e.Id)
            .HasName("pk_department_position");

        builder.Property(e => e.Id)
            .HasConversion(dId => dId.Value, id => DepartmentPositionId.Of(id))
            .HasColumnName("id");

        builder.HasOne<Department>().WithMany(e => e.Positions)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(e => e.DepartmentId).HasColumnName("department_id");

        builder.HasOne<Position>().WithMany()
            .HasForeignKey(e => e.PositionId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(e => e.PositionId).HasColumnName("position_id");
    }
}