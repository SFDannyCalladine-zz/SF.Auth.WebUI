namespace SF.Common.Settings.Repositories.Interfaces
{
    public interface ISettingRepository
    {
        #region Public Methods

        int FindSettingAsInt(string settingName);

        string FindSettingAsString(string settingName);

        #endregion Public Methods
    }
}