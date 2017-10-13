using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SF.Common.Root;

namespace SF.Common.DataAccess.Mappings
{
    public class RootUserMapping : IEntityTypeConfiguration<RootUser>
    {
        #region Public Methods

        public void Configure(EntityTypeBuilder<RootUser> builder)
        {
            builder.ToTable("User");

            builder.HasKey(x => x.UserGuid);

            builder.Property(x => x.UserGuid)
                .HasColumnName("UserGUID")
                .HasColumnType(DatabaseType.UID)
                .IsRequired();

            builder.Property(x => x.Email)
                .HasColumnName("Email")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.AccountGuid)
                .HasColumnName("AccountGUID")
                .HasColumnType(DatabaseType.UID)
                .IsRequired();
        }

        #endregion Public Methods
    }
}