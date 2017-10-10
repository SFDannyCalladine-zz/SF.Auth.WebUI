namespace SF.Auth.WebUI.Models
{
    public class HelpLinkViewModel
    {
        public string LinkText { get; private set; }
        public int Order { get; private set; }
        public string Url { get; private set; }

        public HelpLinkViewModel(
            string url,
            string linkText,
            int order)
        {
            Url = url;
            LinkText = linkText;
            Order = order;
        }
    }
}