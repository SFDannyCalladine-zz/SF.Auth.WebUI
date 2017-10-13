namespace SF.Common.ServiceModels.Response
{
    public class Response
    {
        #region Public Properties

        public ResponseCode Code { get; private set; }

        public string ErrorMessage { get; private set; }

        #endregion Public Properties

        #region Public Constructors

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

        #endregion Public Constructors
    }
}