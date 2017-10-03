using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SF.Common.DataAccess;

namespace SF.Common.Settings.Database.Mapping
{
    public class SettingMapping : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.ToTable("Setting", "dbo");

            builder.HasKey(x => x.SettingId);

            builder.Property(x => x.SettingId)
                .HasColumnName("SettingId")
                .HasColumnType(DatabaseType.TinyInt)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("Name")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Value)
                .HasColumnName("Value")
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}