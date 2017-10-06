using SF.Common.Settings.Database;
using SF.Common.Settings.Repositories.Interfaces;
using System;
using System.Linq;

namespace SF.Common.Settings.Repositories
{
    public class SettingRepository : ISettingRepository
    {
        private readonly dbSetting _context;

        public SettingRepository(dbSetting context)
        {
            _context = context;
        }

        public int FindSettingAsInt(string settingName)
        {
            return Convert.ToInt32(FindSetting(settingName).Value);
        }

        public string FindSettingAsString(string settingName)
        {
            return FindSetting(settingName).Value;
        }

        private Setting FindSetting(string settingName)
        {
            var setting = _context
                .Settings
                .FirstOrDefault(x => x.Name == settingName);

            if (setting == null)
            {
                throw new Exception("Setting can not be found");
            }

            return setting;
        }
    }
}