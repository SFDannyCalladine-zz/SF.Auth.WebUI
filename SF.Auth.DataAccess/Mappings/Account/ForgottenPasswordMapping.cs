using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SF.Auth.Accounts;
using SF.Common.DataAccess;

namespace SF.Auth.DataAccess.Mappings.Account
{
    public class ForgottenPasswordMapping : IEntityTypeConfiguration<ForgottenPassword>
    {
        #region Public Methods

        public void Configure(EntityTypeBuilder<ForgottenPassword> builder)
        {
            builder.ToTable("ForgottenPassword");

            builder.HasKey(x => x.Key);

            builder.Property(x => x.Key)
                .HasColumnName("ResetGUID")
                .HasColumnType(DatabaseType.UID)
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnName("UserId")
                .HasColumnType(DatabaseType.Int)
                .IsRequired();

            builder.Property(x => x.Used)
                .HasColumnName("Used")
                .HasColumnType(DatabaseType.Bit)
                .IsRequired();

            builder.Property(x => x.Created)
                .HasColumnName("Created")
                .HasColumnType(DatabaseType.DateTime)
                .IsRequired();
        }

        #endregion Public Methods
    }
}