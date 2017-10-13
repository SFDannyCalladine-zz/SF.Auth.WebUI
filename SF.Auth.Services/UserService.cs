using SF.Auth.Accounts;
using SF.Auth.DataTransferObjects.Account;
using SF.Auth.Repositories;
using SF.Auth.Repositories.Interfaces;
using SF.Auth.Services.Interfaces;
using SF.Auth.Services.Request;
using SF.Common.DataAccess.Interface;
using SF.Common.Repositories.Interfaces;
using SF.Common.ServiceModels.Response;
using SF.Common.Services;
using SF.Common.Services.Exceptions;
using SF.Common.Settings;
using SF.Common.Settings.Repositories.Interfaces;
using System;

namespace SF.Auth.Services
{
    public class UserService : RootService, IUserService
    {
        private readonly IDbCustomerDatabaseFactory _customerContextFactory;

        private IUserRepository _userRepository;

        public UserService(
            IRootRepository rootRepository,
            ISettingRepository settingRepository,
            IDbCustomerDatabaseFactory customerContextFactory)
            : base(
                  rootRepository,
                  settingRepository)
        {
            _customerContextFactory = customerContextFactory;
        }

        public Response<bool> IsValidKey(IsValidKeyRequest request)
        {
            try
            {
                SetUserRepositoryByUserGuid(request.UserGuid);

                var user = _userRepository.FindByKey(request.Key);

                if (user == null)
                {
                    throw new ServiceException(ResponseCode.NotFound, "Selected Agent does not exist.");
                }

                var expiryLength = _settingRepository.FindSettingAsInt(SettingName.PasswordResetLength);

                var isvalid = user.IsValidKey(request.Key, expiryLength);

                return new Response<bool>(isvalid);
            }
            catch (Exception e)
            {
                return HandleException<bool>(e);
            }
        }

        public Response<RequestPasswordResetDto> RequestPasswordReset(RequestPasswordResetRequest request)
        {
            try
            {
                SetUserRepositoryByEmail(request.EmailAddress);

                var user = _userRepository.FindUserByEmail(request.EmailAddress);

                if (user == null)
                {
                    throw new ServiceException(ResponseCode.NotFound, "Selected Agents does not exist.");
                }

                var expiryLength = _settingRepository.FindSettingAsInt(SettingName.PasswordResetLength);

                var passwordResetRequest = new ForgottenPassword(Guid.NewGuid());

                user.AddPasswordResetRequest(passwordResetRequest, expiryLength);

                _userRepository.Save();

                var dto = new RequestPasswordResetDto
                {
                    UserGuid = user.UserGuid,
                    Key = passwordResetRequest.Key
                };

                return new Response<RequestPasswordResetDto>(dto);
            }
            catch (Exception e)
            {
                return HandleException<RequestPasswordResetDto>(e);
            }
        }

        public Response ResetPassword(ResetPasswordRequest request)
        {
            try
            {
                SetUserRepositoryByUserGuid(request.UserGuid);

                var user = _userRepository.FindByKey(request.Key);

                if (user == null)
                {
                    throw new ServiceException(ResponseCode.NotFound, "Selected Agent does not exist.");
                }

                var expiryLength = _settingRepository.FindSettingAsInt(SettingName.PasswordResetLength);

                user.UpdatePasswordWithKey(
                    request.Key,
                    expiryLength,
                    request.Password);

                _userRepository.Save();

                return new Response(ResponseCode.Success);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        private void SetUserRepositoryByEmail(string emailAddress)
        {
            var connectionString = GetConnectionStringByEmail(emailAddress);

            _userRepository = new UserRepository(_customerContextFactory.CreateDbContext(connectionString));
        }

        private void SetUserRepositoryByUserGuid(Guid userGuid)
        {
            var connectionString = GetConnectionStringByUserGuid(userGuid);

            _userRepository = new UserRepository(_customerContextFactory.CreateDbContext(connectionString));
        }
    }
}