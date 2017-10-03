namespace SF.Common.ServiceModels.Response
{
    public class Response<T> : Response
    {
        public T Entity { get; private set; }

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
    }
}