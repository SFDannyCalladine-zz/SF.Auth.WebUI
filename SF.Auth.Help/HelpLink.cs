namespace SF.Common.Help
{
    public class HelpLink
    {
        public byte HelpLinkId { get; private set; }

        public string LinkText { get; private set; }
        public byte Order { get; private set; }
        public string Url { get; private set; }

        private HelpLink()
        {
        }
    }
}