using SF.Common.Domain.Exceptions;
using SF.Common.Repositories.Interfaces;
using SF.Common.Root;
using SF.Common.Security;
using SF.Common.ServiceModels.Response;
using SF.Common.Services.Exceptions;
using SF.Common.Settings;
using SF.Common.Settings.Repositories.Interfaces;
using System;

namespace SF.Common.Services
{
    public abstract class BaseService
    {
        protected readonly IRootRepository _rootRepository;

        protected readonly ISettingRepository _settingRepository;

        public BaseService(
            IRootRepository rootRepository,
            ISettingRepository settingRepository)
        {
            _rootRepository = rootRepository;
            _settingRepository = settingRepository;
        }

        protected string GetConnectionStringByEmail(string email)
        {
            var connection = _rootRepository.FindConnectionByEmail(email);

            ValidateConnection(connection);

            var connectionString = DecryptConnectionString(connection);

            return connectionString;
        }

        protected string GetConnectionStringByUserGuid(Guid userGuid)
        {
            var connection = _rootRepository.FindConnectionByUserGuid(userGuid);

            ValidateConnection(connection);

            var connectionString = DecryptConnectionString(connection);

            return connectionString;
        }

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

        private static void ValidateConnection(RootConnection connection)
        {
            if (connection == null)
            {
                throw new ServiceException(ResponseCode.NotFound, "Connection for provided User can not be found.");
            }

            if (string.IsNullOrWhiteSpace(connection.EncryptedConnectionString))
            {
                throw new ServiceException(ResponseCode.ValidationError, "Found Connection for provided User does not contain a Connection String.");
            }
        }

        private string DecryptConnectionString(RootConnection connection)
        {
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

            return connectionString;
        }
    }
}