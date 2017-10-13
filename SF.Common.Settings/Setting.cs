namespace SF.Common.Settings
{
    public class Setting
    {
        #region Public Properties

        public string Name { get; private set; }

        public byte SettingId { get; private set; }

        public string Value { get; private set; }

        #endregion Public Properties

        #region Private Constructors

        private Setting()
        {
        }

        #endregion Private Constructors
    }
}