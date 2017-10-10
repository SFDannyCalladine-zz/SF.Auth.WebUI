using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SF.Auth.Accounts;
using SF.Common.DataAccess;

namespace SF.Auth.DataAccess.Mappings.Account
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.UserId);

            builder.Property(x => x.UserId)
                .HasColumnName("UserID")
                .HasColumnType(DatabaseType.Int)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(x => x.UserGuid)
                .HasColumnName("UserGUID")
                .HasColumnType(DatabaseType.UID);

            builder.Property(x => x.Email)
                .HasColumnName("Email")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Password)
                .HasColumnName("Password")
                .HasMaxLength(100);

            builder.Property(x => x.Name)
                .HasColumnName("Name")
                .HasMaxLength(50);

            builder.HasMany(x => x.ForgottenPasswords)
                .WithOne()
                .HasForeignKey(x => x.UserId);
        }
    }
}