namespace SF.Auth.WebUI.Models.Account
{
    public class LogoutViewModel
    {
        public string LogoutId { get; set; }

        public bool ShowLogoutPrompt { get; private set; }

        public LogoutViewModel() { }

        public LogoutViewModel(
            string logoutId,
            bool showLogoutPrompt)
        {
            LogoutId = logoutId;
            ShowLogoutPrompt = showLogoutPrompt;
        }
    }
}
