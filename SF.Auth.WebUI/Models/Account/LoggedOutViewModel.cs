namespace SF.Auth.WebUI.Models.Account
{
    public class LoggedOutViewModel
    {
        public string PostLogoutRedirectUri { get; private set; }

        public string ClientName { get; private set; }

        public string SignOutIframeUrl { get; private set; }

        public bool AutomaticRedirectAfterSignOut { get; private set; }

        public string LogoutId { get; private set; }

        public bool TriggerExternalSignout { get; private set; }

        public string ExternalAuthenticationScheme { get; private set; }

        public LoggedOutViewModel() { }

        public LoggedOutViewModel(
            string logoutId,
            string clientName,
            bool automaticRedirectAfterSignOut,
            string signOutIframeUrl,
            string postLogoutRedirectUri,
            string externalAuthenticationScheme,
            bool triggerExternalSignout)
        {
            logoutId = LogoutId;
            clientName = ClientName;
            AutomaticRedirectAfterSignOut = automaticRedirectAfterSignOut;
            SignOutIframeUrl = signOutIframeUrl;
            PostLogoutRedirectUri = postLogoutRedirectUri;
            ExternalAuthenticationScheme = externalAuthenticationScheme;
            TriggerExternalSignout = triggerExternalSignout;
        }
    }
}
