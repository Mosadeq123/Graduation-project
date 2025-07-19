using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class MachineConfigurations : IEntityTypeConfiguration<MachineEntity>
{
    public void Configure(EntityTypeBuilder<MachineEntity> builder)
    {
        builder.Property(m => m.NameMachine)
            .HasMaxLength(200)
            .IsRequired(); // خاصية إلزامية

        builder.Property(m => m.PictureUrl)
            .HasMaxLength(1000) // الطول الأقصى للخاصية
            .IsRequired(); // خاصية إلزامية

        builder.Property(m => m.Description)
            .HasMaxLength(500); // الطول الأقصى للوصف

        builder.Property(m => m.NeedlesCount)
            .HasMaxLength(100); // الطول الأقصى لعدد الإبر

        builder.Property(m => m.MachineType)
            .HasMaxLength(100); // الطول الأقصى لنوع الماكينة

        builder.Property(m => m.Softness)
            .HasMaxLength(50); // الطول الأقصى للنعومة

        builder.Property(m => m.Width)
            .HasMaxLength(50); // الطول الأقصى للعرض

        // العلاقة مع RawMaterials
        builder.HasMany(m => m.RawMaterials)
            .WithOne(rm => rm.Machine) // تأكد من وجود خاصية Machine في RawMaterial
            .HasForeignKey(rm => rm.MachineId)
            .OnDelete(DeleteBehavior.SetNull); // حذف مرتبط مع تعيين Null
    }
}