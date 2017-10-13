using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SF.Common.Root;

namespace SF.Common.DataAccess.Mappings
{
    public class RootConnectionMapping : IEntityTypeConfiguration<RootConnection>
    {
        #region Public Methods

        public void Configure(EntityTypeBuilder<RootConnection> builder)
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

            builder.HasOne(x => x.Account)
                .WithOne(x => x.Connection)
                .HasForeignKey<RootAccount>(x => x.ConnectionGuid);
        }

        #endregion Public Methods
    }
}