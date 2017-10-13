namespace SF.Common.ServiceModels.Response
{
    public class Response<T> : Response
    {
        #region Public Properties

        public T Entity { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public Response(ResponseCode code, string errorMessage)
            : base(code, errorMessage)
        {
        }

        public Response(ResponseCode code)
            : base(code)
        {
        }

        public Response(T entity)
            : this(ResponseCode.Success)
        {
            Entity = entity;
        }

        #endregion Public Constructors
    }
}