namespace SF.Auth.Services.Request
{
    public class ValidateUserRequest
    {
        public string Email { get; private set; }

        public string Password { get; private set; }

        public ValidateUserRequest(
            string email,
            string password)
        {
            Email = email;
            Password = password;
        }
    }
}