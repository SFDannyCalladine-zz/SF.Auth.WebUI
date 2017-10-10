using AutoMapper;
using SF.Auth.DataTransferObjects.Account;
using SF.Auth.Repositories;
using SF.Auth.Repositories.Interfaces;
using SF.Auth.Services.Interfaces;
using SF.Auth.Services.Request;
using SF.Common.DataAccess.Interface;
using SF.Common.Repositories.Interfaces;
using SF.Common.Security;
using SF.Common.ServiceModels.Response;
using SF.Common.Services;
using SF.Common.Services.Exceptions;
using SF.Common.Settings;
using SF.Common.Settings.Repositories.Interfaces;
using System;

namespace SF.Auth.Services
{
    public class AuthService : RootService, IAuthService
    {
        private readonly IDbCustomerDatabaseFactory _customerContextFactory;
        private readonly IMapper _mapper;

        private IUserRepository _userRepository;

        public AuthService(
            IRootRepository rootRepository,
            ISettingRepository settingRepository,
            IDbCustomerDatabaseFactory customerContextFactory,
            IMapper mapper)
            : base(
                  rootRepository,
                  settingRepository)
        {
            _customerContextFactory = customerContextFactory;
            _mapper = mapper;
        }

        public Response<UserDto> ValidateUser(ValidateUserRequest request)
        {
            try
            {
                var connection = _rootRepository.FindConnectionByEmail(request.Email);

                if (connection == null)
                {
                    throw new ServiceException(ResponseCode.NotFound, "Connection can not be found for provided Username.");
                }

                var encryptionSaltSetting = _settingRepository.FindSettingAsString(SettingName.EncryptionSalt);
                var encryptionKeySetting = _settingRepository.FindSettingAsString(SettingName.EncryptionKey);

                if (string.IsNullOrWhiteSpace(encryptionSaltSetting))
                {
                    throw new ServiceException(ResponseCode.NotFound, "Encryption Salt can not be found.");
                }

                if (string.IsNullOrWhiteSpace(encryptionKeySetting))
                {
                    throw new ServiceException(ResponseCode.NotFound, "Encryption Key can not be found.");
                }

                var connectionString =
                    new Encryption(
                        encryptionKeySetting,
                        encryptionSaltSetting,
                        connection.ConnectionGuid.ToByteArray())
                    .DecryptString(connection.EncryptedConnectionString);

                _userRepository = new UserRepository(_customerContextFactory.CreateDbContext(connectionString));

                var user = _userRepository.FindUserByEmail(request.Email);

                if (user == null)
                {
                    throw new ServiceException(ResponseCode.NotFound, "User can not be found with provided Email.");
                }

                var valid = user.ValidatePassword(request.Password);

                if (valid)
                {
                    var dto = _mapper.Map<UserDto>(user);

                    return new Response<UserDto>(dto);
                }

                return new Response<UserDto>(ResponseCode.ValidationError, "Failed Login");
            }
            catch (Exception e)
            {
                return HandleException<UserDto>(e);
            }
        }
    }
}