namespace SF.Common.Settings
{
    public class Setting
    {
        public string Name { get; private set; }

        public byte SettingId { get; private set; }

        public string Value { get; private set; }

        private Setting()
        {
        }
    }
}