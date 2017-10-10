using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SF.Common.DataAccess;
using SF.Common.Help;

namespace SF.Auth.DataAccess.Mappings.Help
{
    internal class HelpLinkMapping : IEntityTypeConfiguration<HelpLink>
    {
        public void Configure(EntityTypeBuilder<HelpLink> builder)
        {
            builder.ToTable("HelpLinks");

            builder.HasKey(x => x.HelpLinkId);

            builder.Property(x => x.HelpLinkId)
                .HasColumnName("HelpLinkID")
                .HasColumnType(DatabaseType.TinyInt)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(x => x.Url)
                .HasColumnName("Url")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.LinkText)
                .HasColumnName("LinkText")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Order)
                .HasColumnName("Order")
                .HasColumnType(DatabaseType.TinyInt)
                .IsRequired();
        }
    }
}