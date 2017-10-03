using AutoMapper;
using SF.Auth.DataTransferObjects;
using SF.Auth.Repositories;
using SF.Auth.Repositories.Interfaces;
using SF.Auth.Services.Interfaces;
using SF.Auth.Services.Request;
using SF.Common.DataAccess.Interface;
using SF.Common.Security;
using SF.Common.ServiceModels.Response;
using SF.Common.Services;
using SF.Common.Services.Exceptions;
using SF.Common.Settings;
using SF.Common.Settings.Repositories.Interfaces;
using System;

namespace SF.Auth.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IAuthRepository _authRepository;

        private readonly IDbCustomerDatabaseFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ISettingRepository _settingRepository;

        private ICustomerAuthRepository _customerAuthRepository;

        public AuthService(
            IAuthRepository authRepository,
            ISettingRepository settingRepository,
            IDbCustomerDatabaseFactory contextFactory,
            IMapper mapper)
        {
            _authRepository = authRepository;
            _settingRepository = settingRepository;
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public Response<ConnectionDto> FindConnectionByEmail(string email)
        {
            try
            {
                var connection = _authRepository.FindConnectionByEmail(email);

                if (connection == null)
                {
                    throw new ServiceException(ResponseCode.NotFound, "Connection can not be found for provided Username");
                }

                var dto = _mapper.Map<ConnectionDto>(connection);

                return new Response<ConnectionDto>(dto);
            }
            catch (Exception e)
            {
                return HandleException<ConnectionDto>(e);
            }
        }

        public Response<UserDto> ValidateUser(ValidateUserRequest request)
        {
            try
            {
                var connection = _authRepository.FindConnectionByEmail(request.Email);

                if (connection == null)
                {
                    throw new ServiceException(ResponseCode.NotFound, "Connection can not be found for provided Username.");
                }

                var encryptionSaltSetting = _settingRepository.FindSetting(SettingName.EncryptionSalt);
                var encryptionKeySetting = _settingRepository.FindSetting(SettingName.EncryptionKey);

                if (encryptionSaltSetting == null)
                {
                    throw new ServiceException(ResponseCode.NotFound, "Encryption Salt can not be found.");
                }

                if (encryptionKeySetting == null)
                {
                    throw new ServiceException(ResponseCode.NotFound, "Encryption Key can not be found.");
                }

                var connectionString =
                    new Encryption(
                        encryptionKeySetting.Value,
                        encryptionSaltSetting.Value,
                        connection.ConnectionGuid.ToByteArray())
                    .DecryptString(connection.EncryptedConnectionString);

                _customerAuthRepository = new CustomerAuthRepository(_contextFactory.CreateDbContext(connectionString));

                var user = _customerAuthRepository.FindUserByEmail(request.Email);

                if (user == null)
                {
                    throw new ServiceException(ResponseCode.NotFound, "User can not be found with provided Email.");
                }

                var valid = Hashing.Validate(request.Password, user.Password);

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