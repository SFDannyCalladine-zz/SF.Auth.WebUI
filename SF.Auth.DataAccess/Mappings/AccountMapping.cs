using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SF.Auth.Accounts;
using SF.Common.DataAccess;

namespace SF.Auth.DataAccess.Mappings
{
    public class AccountMapping : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
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

            builder.HasMany(x => x.Users).WithOne().HasForeignKey(x => x.AccountGuid);
            builder.HasOne(x => x.Connection).WithOne().HasForeignKey<Account>(x => x.ConnectionGuid);
        }
    }
}