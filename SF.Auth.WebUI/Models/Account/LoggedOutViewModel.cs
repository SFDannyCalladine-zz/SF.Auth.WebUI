namespace SF.Auth.WebUI.Models.Account
{
    public class LoggedOutViewModel
    {
        #region Public Properties

        public string ClientName { get; private set; }
        public string LogoutId { get; private set; }
        public string PostLogoutRedirectUri { get; private set; }
        public string SignOutIframeUrl { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public LoggedOutViewModel()
        {
        }

        public LoggedOutViewModel(
            string logoutId,
            string clientName,
            string signOutIframeUrl,
            string postLogoutRedirectUri)
        {
            LogoutId = logoutId;
            ClientName = clientName;
            SignOutIframeUrl = signOutIframeUrl;
            PostLogoutRedirectUri = postLogoutRedirectUri;
        }

        #endregion Public Constructors
    }
}