namespace SF.Auth.Services.Request
{
    public class RequestPasswordResetRequest
    {
        public string EmailAddress { get; private set; }

        public RequestPasswordResetRequest(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
    }
}