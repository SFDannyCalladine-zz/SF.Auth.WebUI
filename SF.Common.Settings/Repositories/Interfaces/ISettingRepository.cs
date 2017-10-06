namespace SF.Common.Settings.Repositories.Interfaces
{
    public interface ISettingRepository
    {
        int FindSettingAsInt(string settingName);

        string FindSettingAsString(string settingName);
    }
}