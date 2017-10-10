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
    public class RootService : BaseService
    {
        protected readonly IRootRepository _rootRepository;

        protected readonly ISettingRepository _settingRepository;

        public RootService(
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