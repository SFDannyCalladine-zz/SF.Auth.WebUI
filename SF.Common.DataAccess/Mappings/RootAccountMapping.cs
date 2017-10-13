using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SF.Common.Root;

namespace SF.Common.DataAccess.Mappings
{
    public class RootAccountMapping : IEntityTypeConfiguration<RootAccount>
    {
        #region Public Methods

        public void Configure(EntityTypeBuilder<RootAccount> builder)
        {
            builder.ToTable("Account");

            builder.HasKey(x => x.AccountGuid);

            builder.Property(x => x.AccountGuid)
                .HasColumnName("AccountGUID")
                .HasColumnType(DatabaseType.UID)
                .IsRequired();

            builder.Property(x => x.ConnectionGuid)
                .HasColumnName("ConnectionGUID")
                .HasColumnType(DatabaseType.UID)
                .IsRequired();

            builder.HasMany(x => x.Users)
                .WithOne()
                .HasForeignKey(x => x.AccountGuid);

            builder.HasOne(x => x.Connection)
                .WithOne()
                .HasForeignKey<RootAccount>(x => x.ConnectionGuid);
        }

        #endregion Public Methods
    }
}