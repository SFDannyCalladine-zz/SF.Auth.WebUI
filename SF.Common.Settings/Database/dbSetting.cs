using Microsoft.EntityFrameworkCore;
using SF.Common.Settings.Database.Mapping;

namespace SF.Common.Settings.Database
{
    public class dbSetting : DbContext
    {
        #region Public Properties

        public DbSet<Setting> Settings { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public dbSetting(DbContextOptions<dbSetting> options)
            : base(options)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new SettingMapping());

            base.OnModelCreating(builder);
        }

        #endregion Protected Methods
    }
}