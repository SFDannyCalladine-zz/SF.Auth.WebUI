using SF.Auth.DataTransferObjects.Account;
using SF.Auth.Services.Request;
using SF.Common.ServiceModels.Response;

namespace SF.Auth.Services.Interfaces
{
    public interface IAuthService
    {
        #region Public Methods

        Response<UserDto> ValidateUser(ValidateUserRequest request);

        #endregion Public Methods
    }
}