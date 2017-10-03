using Microsoft.EntityFrameworkCore;
using SF.Common.Settings.Database.Mapping;

namespace SF.Common.Settings.Database
{
    public class dbSetting : DbContext
    {
        public DbSet<Setting> Settings { get; set; }

        public dbSetting(DbContextOptions<dbSetting> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new SettingMapping());

            base.OnModelCreating(builder);
        }
    }
}