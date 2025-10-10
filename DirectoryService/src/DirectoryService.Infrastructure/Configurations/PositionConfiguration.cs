using DirectoryService.Domain.Positions;
using DirectoryService.Domain.Positions.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configurations;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
     {
         builder.ToTable("positions");

         builder.HasKey(e => e.Id)
             .HasName("pk_position");

         builder.Property(e => e.Id)
             .HasConversion(pId => pId.Value, id => PositionId.Of(id))
             .HasColumnName("id");

         builder.ComplexProperty(e => e.Name, nb =>
         {
             nb.Property(n => n.Value)
                 .HasColumnName("name")
                 .HasMaxLength(PositionName.MAX_LENGHT)
                 .IsRequired();
         });

         builder.ComplexProperty(e => e.Description, db =>
         {
             db.Property(n => n.Value)
                 .HasColumnName("description")
                 .HasMaxLength(Description.MAX_LENGHT)
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