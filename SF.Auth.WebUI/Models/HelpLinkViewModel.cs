namespace SF.Auth.WebUI.Models
{
    public class HelpLinkViewModel
    {
        #region Public Properties

        public string LinkText { get; private set; }
        public int Order { get; private set; }
        public string Url { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public HelpLinkViewModel(
            string url,
            string linkText,
            int order)
        {
            Url = url;
            LinkText = linkText;
            Order = order;
        }

        #endregion Public Constructors
    }
}