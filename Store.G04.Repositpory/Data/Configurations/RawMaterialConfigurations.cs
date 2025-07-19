using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.G04.Core.Entities;

namespace Store.G04.Repositpory.Data.Configurations
{
    public class RawMaterialConfigurations : IEntityTypeConfiguration<RawMaterial>
    {
        public void Configure(EntityTypeBuilder<RawMaterial> builder)
        {
            builder.Property(r => r.NameMaterial)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(r => r.Description)
                .HasMaxLength(500);

            builder.Property(r => r.Quantity)
                .HasMaxLength(50);

            builder.Property(r => r.StitchLength)
                .HasMaxLength(50);

            builder.Property(r => r.YarnType)
                .HasMaxLength(100);

            builder.Property(r => r.PictureUrl)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(r => r.UnitPrice)
                .HasColumnType("decimal(18,2)") // Specify the column type with precision and scale
                .IsRequired();

            builder.HasOne(r => r.Machine)
                .WithMany(m => m.RawMaterials)
                .HasForeignKey(r => r.MachineId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
