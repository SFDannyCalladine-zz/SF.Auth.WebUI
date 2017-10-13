using System;
using System.Linq;
using SF.Auth.Repositories.Cache;
using SF.Common.Repositories.Cache.Interfaces;
using SF.Common.Settings.Database;
using SF.Common.Settings.Repositories.Interfaces;

namespace SF.Common.Settings.Repositories
{
    public class SettingRepository : CacheRepository, ISettingRepository
    {
        #region Private Fields

        private readonly dbSetting _context;

        #endregion Private Fields

        #region Public Constructors

        public SettingRepository
            (dbSetting context,
             ICacheStorage cache)
            : base(cache)
        {
            _context = context;
        }

        #endregion Public Constructors

        #region Public Methods

        public int FindSettingAsInt(string settingName)
        {
            return Convert.ToInt32(FindSetting(settingName).Value);
        }

        public string FindSettingAsString(string settingName)
        {
            return FindSetting(settingName).Value;
        }

        #endregion Public Methods

        #region Private Methods

        private Setting FindSetting(string settingName)
        {
            var setting = Cache.Retrieve<Setting>(settingName);

            if (setting == null)
            {
                setting = _context
                    .Settings
                    .FirstOrDefault(x => x.Name == settingName);

                if (setting == null)
                {
                    throw new Exception("Setting can not be found");
                }

                Cache.Store(setting.Name, setting.Value);
            }

            return setting;
        }

        #endregion Private Methods
    }
}