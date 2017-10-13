namespace SF.Auth.WebUI.Models.Account
{
    public class LogoutViewModel
    {
        #region Public Properties

        public string LogoutId { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public LogoutViewModel()
        {
        }

        public LogoutViewModel(string logoutId)
        {
            LogoutId = logoutId;
        }

        #endregion Public Constructors
    }
}