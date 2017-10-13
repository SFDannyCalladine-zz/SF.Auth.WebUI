namespace SF.Auth.Services.Request
{
    public class ValidateUserRequest
    {
        #region Public Properties

        public string Email { get; private set; }

        public string Password { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public ValidateUserRequest(
            string email,
            string password)
        {
            Email = email;
            Password = password;
        }

        #endregion Public Constructors
    }
}