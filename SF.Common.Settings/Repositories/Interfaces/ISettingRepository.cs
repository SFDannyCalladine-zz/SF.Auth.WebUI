namespace SF.Common.Settings.Repositories.Interfaces
{
    public interface ISettingRepository
    {
        Setting FindSetting(string settingName);
    }
}