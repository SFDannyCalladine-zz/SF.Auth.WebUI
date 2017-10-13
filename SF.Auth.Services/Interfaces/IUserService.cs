using SF.Auth.DataTransferObjects.Account;
using SF.Auth.Services.Request;
using SF.Common.ServiceModels.Response;

namespace SF.Auth.Services.Interfaces
{
    public interface IUserService
    {
        Response<bool> IsValidKey(IsValidKeyRequest request);

        Response<RequestPasswordResetDto> RequestPasswordReset(RequestPasswordResetRequest request);

        Response ResetPassword(ResetPasswordRequest request);
    }
}