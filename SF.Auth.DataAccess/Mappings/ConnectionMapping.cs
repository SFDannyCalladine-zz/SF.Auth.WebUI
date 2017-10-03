using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SF.Auth.Accounts;
using SF.Common.DataAccess;

namespace SF.Auth.DataAccess.Mappings
{
    public class ConnectionMapping : IEntityTypeConfiguration<Connection>
    {
        public void Configure(EntityTypeBuilder<Connection> builder)
        {
            builder.ToTable("Connection");

            builder.HasKey(x => x.ConnectionGuid);

            builder.Property(x => x.ConnectionGuid)
                .HasColumnName("ConnectionGUID")
                .HasColumnType(DatabaseType.UID)
                .IsRequired();

            builder.Property(x => x.EncryptedConnectionString)
                .HasColumnName("EncryptedConnectionString")
                .IsRequired();

            builder.HasOne(x => x.Account).WithOne(x => x.Connection).HasForeignKey<Account>(x => x.ConnectionGuid);
        }
    }
}