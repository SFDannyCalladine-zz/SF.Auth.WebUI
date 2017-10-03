using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SF.Auth.Accounts;
using SF.Common.DataAccess;

namespace SF.Auth.DataAccess.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
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
    }
}