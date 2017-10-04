namespace SF.Auth.WebUI.Models.Account
{
    public class LogoutViewModel
    {
        public string LogoutId { get; set; }

        public LogoutViewModel()
        {
        }

        public LogoutViewModel(string logoutId)
        {
            LogoutId = logoutId;
        }
    }
}