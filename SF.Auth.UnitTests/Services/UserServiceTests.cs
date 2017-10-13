using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SF.Auth.Accounts;
using SF.Auth.DataAccess;
using SF.Auth.Services;
using SF.Auth.Services.Request;
using SF.Common.DataAccess.Interface;
using SF.Common.Repositories.Interfaces;
using SF.Common.Root;
using SF.Common.Security;
using SF.Common.ServiceModels.Response;
using SF.Common.Settings;
using SF.Common.Settings.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SF.Auth.UnitTests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private const string ForgottenPasswordGuid = "d95eb2f3-d9fb-4143-b046-f9cc34491f65";
        private const string Key = "2013.mcw.24ge2$ed[@dysSD62Lpww#$";
        private const string Salt = "2013.mcw.suhg$^s68#7IUHd98$uw09i";
        private const string UsedForgottenPasswordGuid = "9bb772a4-ab50-491d-ab3c-04cfadc6b092";
        private const string UserGuid = "0a4c93ce-1b09-4a07-9eb2-7ec200298292";
        private RootConnection _connection;
        private Mock<IDbCustomerDatabaseFactory> _customerContextFactoryMock;
        private Mock<dbCustomerDatabase> _customerDatabaseMock;
        private IsValidKeyRequest _isValidKeyRequest;
        private RequestPasswordResetRequest _requestPasswordResetRequest;
        private ResetPasswordRequest _resetPasswordRequest;
        private Mock<IRootRepository> _rootRepoMock;
        private Mock<ISettingRepository> _settingRepoMock;
        private Mock<DbSet<User>> _userDbSetMock;
        private UserService _userService;

        [Test]
        public void IsValidKeyRepositoryExceptionTest()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByUserGuid(It.IsAny<Guid>())).Throws<Exception>();

            var response = _userService.IsValidKey(_isValidKeyRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.ServerError, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
            Assert.AreEqual(false, response.Entity);
        }

        [Test]
        public void IsValidKeyServiceExceptionEncryptionKeySettingNotFound()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByUserGuid(It.IsAny<Guid>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns(Salt);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns((string)null);

            var response = _userService.IsValidKey(_isValidKeyRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.NotFound, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
            Assert.AreEqual(false, response.Entity);
        }

        [Test]
        public void IsValidKeyServiceExceptionEncryptionSaltSettingNotFound()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByUserGuid(It.IsAny<Guid>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns((string)null);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns(Key);

            var response = _userService.IsValidKey(_isValidKeyRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.NotFound, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
            Assert.AreEqual(false, response.Entity);
        }

        [Test]
        public void IsValidKeyServiceExceptionNoConnectionFound()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByUserGuid(It.IsAny<Guid>())).Returns((RootConnection)null);

            var response = _userService.IsValidKey(_isValidKeyRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.NotFound, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
            Assert.AreEqual(false, response.Entity);
        }

        [Test]
        public void IsValidKeyServiceExceptionUserNotFoundTest()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByUserGuid(It.IsAny<Guid>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns(Salt);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns(Key);
            _customerContextFactoryMock.Setup(x => x.CreateDbContext(It.IsAny<string>())).Returns(_customerDatabaseMock.Object);

            _isValidKeyRequest = new IsValidKeyRequest(Guid.NewGuid(), new Guid(UserGuid));

            var response = _userService.IsValidKey(_isValidKeyRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.NotFound, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
            Assert.AreEqual(false, response.Entity);
        }

        [Test]
        public void IsValidKeySuccessfulInvalidTest()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByUserGuid(It.IsAny<Guid>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns(Salt);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns(Key);
            _customerContextFactoryMock.Setup(x => x.CreateDbContext(It.IsAny<string>())).Returns(_customerDatabaseMock.Object);
            _settingRepoMock.Setup(x => x.FindSettingAsInt(SettingName.PasswordResetLength)).Returns(0);

            var response = _userService.IsValidKey(_isValidKeyRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.Success, response.Code);
            Assert.IsEmpty(response.ErrorMessage);
            Assert.IsNotNull(response.Entity);
            Assert.AreEqual(false, response.Entity);
        }

        [Test]
        public void IsValidKeySuccessfulValidTest()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByUserGuid(It.IsAny<Guid>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns(Salt);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns(Key);
            _customerContextFactoryMock.Setup(x => x.CreateDbContext(It.IsAny<string>())).Returns(_customerDatabaseMock.Object);
            _settingRepoMock.Setup(x => x.FindSettingAsInt(SettingName.PasswordResetLength)).Returns(1);

            var response = _userService.IsValidKey(_isValidKeyRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.Success, response.Code);
            Assert.IsEmpty(response.ErrorMessage);
            Assert.IsNotNull(response.Entity);
            Assert.AreEqual(true, response.Entity);
        }

        [Test]
        public void RequestPasswordResetRepositoryExceptionTest()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByEmail(It.IsAny<string>())).Throws<Exception>();

            var response = _userService.RequestPasswordReset(_requestPasswordResetRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.ServerError, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
            Assert.IsNull(response.Entity);
        }

        [Test]
        public void RequestPasswordResetServiceExceptionEncryptionKeySettingNotFound()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByEmail(It.IsAny<string>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns(Salt);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns((string)null);

            var response = _userService.RequestPasswordReset(_requestPasswordResetRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.NotFound, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
            Assert.IsNull(response.Entity);
        }

        [Test]
        public void RequestPasswordResetServiceExceptionEncryptionSaltSettingNotFound()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByEmail(It.IsAny<string>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns((string)null);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns(Key);

            var response = _userService.RequestPasswordReset(_requestPasswordResetRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.NotFound, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
            Assert.IsNull(response.Entity);
        }

        [Test]
        public void RequestPasswordResetServiceExceptionNoConnectionFound()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByEmail(It.IsAny<string>())).Returns((RootConnection)null);

            var response = _userService.RequestPasswordReset(_requestPasswordResetRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.NotFound, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
            Assert.IsNull(response.Entity);
        }

        [Test]
        public void RequestPasswordResetServiceExceptionUserNotFoundTest()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByEmail(It.IsAny<string>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns(Salt);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns(Key);
            _customerContextFactoryMock.Setup(x => x.CreateDbContext(It.IsAny<string>())).Returns(_customerDatabaseMock.Object);

            _requestPasswordResetRequest = new RequestPasswordResetRequest("Wrong Email");

            var response = _userService.RequestPasswordReset(_requestPasswordResetRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.NotFound, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
            Assert.IsNull(response.Entity);
        }

        [Test]
        public void RequestPasswordResetSuccessfulTest()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByEmail(It.IsAny<string>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns(Salt);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns(Key);
            _customerContextFactoryMock.Setup(x => x.CreateDbContext(It.IsAny<string>())).Returns(_customerDatabaseMock.Object);
            _settingRepoMock.Setup(x => x.FindSettingAsInt(SettingName.PasswordResetLength)).Returns(5);

            var response = _userService.RequestPasswordReset(_requestPasswordResetRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.Success, response.Code);
            Assert.IsEmpty(response.ErrorMessage);
            Assert.IsNotNull(response.Entity);
            Assert.IsNotNull(response.Entity.Key);
            Assert.IsNotNull(response.Entity.UserGuid);
        }

        [Test]
        public void ResetPasswordDomainValidationExceptionCanNotFindForgottenPasswordTest()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByUserGuid(It.IsAny<Guid>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns(Salt);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns(Key);
            _customerContextFactoryMock.Setup(x => x.CreateDbContext(It.IsAny<string>())).Returns(_customerDatabaseMock.Object);
            _settingRepoMock.Setup(x => x.FindSettingAsInt(SettingName.PasswordResetLength)).Returns(1);

            _resetPasswordRequest = new ResetPasswordRequest(Guid.NewGuid(), "Password2", new Guid(UserGuid));

            var response = _userService.ResetPassword(_resetPasswordRequest);

            Assert.IsNotNull(response);
            Assert.AreNotEqual(ResponseCode.Success, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
        }

        public void ResetPasswordDomainValidationExceptionEmptyPasswordTest()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByUserGuid(It.IsAny<Guid>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns(Salt);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns(Key);
            _customerContextFactoryMock.Setup(x => x.CreateDbContext(It.IsAny<string>())).Returns(_customerDatabaseMock.Object);
            _settingRepoMock.Setup(x => x.FindSettingAsInt(SettingName.PasswordResetLength)).Returns(0);

            _resetPasswordRequest = new ResetPasswordRequest(new Guid(ForgottenPasswordGuid), null, new Guid(UserGuid));

            var response = _userService.ResetPassword(_resetPasswordRequest);

            Assert.IsNotNull(response);
            Assert.AreNotEqual(ResponseCode.Success, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
        }

        public void ResetPasswordDomainValidationExceptionExpiredForgottenPasswordTest()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByUserGuid(It.IsAny<Guid>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns(Salt);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns(Key);
            _customerContextFactoryMock.Setup(x => x.CreateDbContext(It.IsAny<string>())).Returns(_customerDatabaseMock.Object);
            _settingRepoMock.Setup(x => x.FindSettingAsInt(SettingName.PasswordResetLength)).Returns(0);

            _resetPasswordRequest = new ResetPasswordRequest(new Guid(ForgottenPasswordGuid), "Password2", new Guid(UserGuid));

            var response = _userService.ResetPassword(_resetPasswordRequest);

            Assert.IsNotNull(response);
            Assert.AreNotEqual(ResponseCode.Success, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
        }

        public void ResetPasswordDomainValidationExceptionUsedForgottenPasswordTest()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByUserGuid(It.IsAny<Guid>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns(Salt);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns(Key);
            _customerContextFactoryMock.Setup(x => x.CreateDbContext(It.IsAny<string>())).Returns(_customerDatabaseMock.Object);
            _settingRepoMock.Setup(x => x.FindSettingAsInt(SettingName.PasswordResetLength)).Returns(0);

            _resetPasswordRequest = new ResetPasswordRequest(new Guid(UsedForgottenPasswordGuid), "Password2", new Guid(UserGuid));

            var response = _userService.ResetPassword(_resetPasswordRequest);

            Assert.IsNotNull(response);
            Assert.AreNotEqual(ResponseCode.Success, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
        }

        [Test]
        public void ResetPasswordRepositoryExceptionTest()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByUserGuid(It.IsAny<Guid>())).Throws<Exception>();

            var response = _userService.ResetPassword(_resetPasswordRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.ServerError, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
        }

        [Test]
        public void ResetPasswordServiceExceptionEncryptionKeySettingNotFound()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByUserGuid(It.IsAny<Guid>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns(Salt);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns((string)null);

            var response = _userService.ResetPassword(_resetPasswordRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.NotFound, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
        }

        [Test]
        public void ResetPasswordServiceExceptionEncryptionSaltSettingNotFound()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByUserGuid(It.IsAny<Guid>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns((string)null);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns(Key);

            var response = _userService.ResetPassword(_resetPasswordRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.NotFound, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
        }

        [Test]
        public void ResetPasswordServiceExceptionNoConnectionFound()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByUserGuid(It.IsAny<Guid>())).Returns((RootConnection)null);

            var response = _userService.ResetPassword(_resetPasswordRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.NotFound, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
        }

        [Test]
        public void ResetPasswordServiceExceptionUserNotFoundTest()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByUserGuid(It.IsAny<Guid>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns(Salt);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns(Key);
            _customerContextFactoryMock.Setup(x => x.CreateDbContext(It.IsAny<string>())).Returns(_customerDatabaseMock.Object);

            _resetPasswordRequest = new ResetPasswordRequest(Guid.NewGuid(), "Password2", new Guid(UserGuid));

            var response = _userService.ResetPassword(_resetPasswordRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.NotFound, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
        }

        [Test]
        public void ResetPasswordSuccessfulValidTest()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByUserGuid(It.IsAny<Guid>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns(Salt);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns(Key);
            _customerContextFactoryMock.Setup(x => x.CreateDbContext(It.IsAny<string>())).Returns(_customerDatabaseMock.Object);
            _settingRepoMock.Setup(x => x.FindSettingAsInt(SettingName.PasswordResetLength)).Returns(1);

            var response = _userService.ResetPassword(_resetPasswordRequest);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.Success, response.Code);
            Assert.IsEmpty(response.ErrorMessage);
        }

        [SetUp]
        public void SetUp()
        {
            _rootRepoMock = new Mock<IRootRepository>();
            _settingRepoMock = new Mock<ISettingRepository>();
            _customerContextFactoryMock = new Mock<IDbCustomerDatabaseFactory>();
            _customerDatabaseMock = new Mock<dbCustomerDatabase>();
            _userDbSetMock = new Mock<DbSet<User>>();

            var user = new User(new Guid(UserGuid), "Name", "Email", "Password");
            user.AddPasswordResetRequest(new ForgottenPassword(new Guid(UsedForgottenPasswordGuid)), 1);
            user.AddPasswordResetRequest(new ForgottenPassword(new Guid(ForgottenPasswordGuid)), 1);

            //Having to due all of this due to multi-tenency set up, I can't inject UserRepo
            var data = new List<User>
            {
                user
            }
            .AsQueryable();

            _userDbSetMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            _userDbSetMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            _userDbSetMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _userDbSetMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _customerDatabaseMock.Setup(c => c.Users).Returns(_userDbSetMock.Object);

            _userService = new UserService(
                _rootRepoMock.Object,
                _settingRepoMock.Object,
                _customerContextFactoryMock.Object);

            _isValidKeyRequest = new IsValidKeyRequest(new Guid(ForgottenPasswordGuid), new Guid(UserGuid));
            _requestPasswordResetRequest = new RequestPasswordResetRequest("Email");
            _resetPasswordRequest = new ResetPasswordRequest(new Guid(ForgottenPasswordGuid), "Password2", new Guid(UserGuid));

            var guid = Guid.NewGuid();
            var connectionString =
                    new Encryption(
                        Key,
                        Salt,
                        guid.ToByteArray())
                    .EncryptString("Connection String");

            _connection = new RootConnection(guid, connectionString);
        }
    }
}