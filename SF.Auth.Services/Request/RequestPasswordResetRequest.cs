namespace SF.Auth.Services.Request
{
    public class RequestPasswordResetRequest
    {
        #region Public Properties

        public string EmailAddress { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public RequestPasswordResetRequest(string emailAddress)
        {
            EmailAddress = emailAddress;
        }

        #endregion Public Constructors
    }
}