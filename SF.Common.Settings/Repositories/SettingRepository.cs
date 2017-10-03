using SF.Common.Settings.Database;
using SF.Common.Settings.Repositories.Interfaces;
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

        public Setting FindSetting(string settingName)
        {
            return _context
                .Settings
                .FirstOrDefault(x => x.Name == settingName);
        }
    }
}