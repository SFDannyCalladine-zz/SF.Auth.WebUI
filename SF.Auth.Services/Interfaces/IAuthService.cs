using SF.Auth.DataTransferObjects;
using SF.Auth.Services.Request;
using SF.Common.ServiceModels.Response;

namespace SF.Auth.Services.Interfaces
{
    public interface IAuthService
    {
        Response<ConnectionDto> FindConnectionByEmail(string email);

        Response<UserDto> ValidateUser(ValidateUserRequest request);
    }
}