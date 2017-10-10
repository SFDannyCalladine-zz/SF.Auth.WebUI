using SF.Common.Domain.Exceptions;
using SF.Common.ServiceModels.Response;
using SF.Common.Services.Exceptions;
using System;

namespace SF.Common.Services
{
    public abstract class BaseService
    {
        protected Response<T> HandleException<T>(Exception e)
        {
            if (e is DomainValidationException)
            {
                return new Response<T>(ResponseCode.ValidationError, e.Message);
            }

            var serviceException = e as ServiceException;

            if (serviceException != null)
            {
                return new Response<T>(serviceException.Code, serviceException.Message);
            }

            return new Response<T>(ResponseCode.ServerError, e.Message);
        }

        protected Response HandleException(Exception e)
        {
            if (e is DomainValidationException)
            {
                return new Response(ResponseCode.ValidationError, e.Message);
            }

            var serviceException = e as ServiceException;

            if (serviceException != null)
            {
                return new Response(serviceException.Code, serviceException.Message);
            }

            return new Response(ResponseCode.ServerError, e.Message);
        }
    }
}