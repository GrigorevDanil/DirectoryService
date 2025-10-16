using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Departments.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");

        builder.HasKey(e => e.Id)
            .HasName("pk_department");

        builder.Property(e => e.Id)
            .HasConversion(dId => dId.Value, id => DepartmentId.Of(id))
            .HasColumnName("id");

        builder.OwnsOne(e => e.Name, nb =>
        {
            nb.Property(n => n.Value)
                .HasColumnName("name")
                .HasMaxLength(DepartmentName.MAX_LENGHT)
                .IsRequired();

            nb.HasIndex(e => e.Value).IsUnique()
                .HasDatabaseName("ix_departments_name");
        });

        builder.OwnsOne(e => e.Identifier, ib =>
        {
            ib.Property(i => i.Value)
                .HasColumnName("identifier")
                .HasMaxLength(Identifier.MAX_LENGHT)
                .IsRequired();

            ib.HasIndex(e => e.Value).IsUnique()
                .HasDatabaseName("ix_departments_identifier");
        });

        builder.OwnsOne(e => e.Path, pb =>
        {
            pb.Property(p => p.Value)
                .HasColumnName("path")
                .IsRequired();

            pb.Property(p => p.Depth)
                .HasColumnName("depth")
                .IsRequired();

            pb.HasIndex(e => e.Value).IsUnique()
                .HasDatabaseName("ix_departments_path");
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

        builder.HasOne<Department>().WithMany(e => e.Children)
            .HasForeignKey(e => e.ParentId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.Property(e => e.ParentId).HasColumnName("parent_id");
    }
}