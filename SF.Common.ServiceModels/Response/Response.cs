namespace SF.Common.ServiceModels.Response
{
    public class Response
    {
        public ResponseCode Code { get; private set; }

        public string ErrorMessage { get; private set; }

        public Response(
            ResponseCode code,
            string errorMessage)
        {
            ErrorMessage = errorMessage;
            Code = code;
        }

        public Response(ResponseCode code)
            : this(
                  code,
                  string.Empty)
        {
        }
    }
}