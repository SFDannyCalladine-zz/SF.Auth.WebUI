using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SF.Auth.Accounts;
using SF.Auth.DataAccess;
using SF.Auth.DataTransferObjects.Account;
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
    public class AuthServiceTests
    {
        private AuthService _authService;

        private Mock<IRootRepository> _rootRepoMock;

        private Mock<ISettingRepository> _settingRepoMock;

        private Mock<IDbCustomerDatabaseFactory> _customerContextFactoryMock;

        private Mock<IMapper> _mapperMock;

        private Mock<dbCustomerDatabase> _customerDatabaseMock;
        private Mock<DbSet<User>> _userDbSetMock;

        private ValidateUserRequest _request;

        private RootConnection _connection;

        private UserDto _userDto;

        private const string Salt = "2013.mcw.suhg$^s68#7IUHd98$uw09i";
        private const string Key = "2013.mcw.24ge2$ed[@dysSD62Lpww#$";

        [SetUp]
        public void SetUp()
        {
            _rootRepoMock = new Mock<IRootRepository>();
            _settingRepoMock = new Mock<ISettingRepository>();
            _customerContextFactoryMock = new Mock<IDbCustomerDatabaseFactory>();
            _mapperMock = new Mock<IMapper>();
            _customerDatabaseMock = new Mock<dbCustomerDatabase>();
            _userDbSetMock = new Mock<DbSet<User>>();

            //Having to due all of this due to multi-tenency set up, I can't inject UserRepo
            var data = new List<User>
            {
                new User(Guid.NewGuid(), "Name", "Email", "Password")
            }
            .AsQueryable();

            _userDbSetMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            _userDbSetMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            _userDbSetMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _userDbSetMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _customerDatabaseMock.Setup(c => c.Users).Returns(_userDbSetMock.Object);

            _authService = new AuthService(
                _rootRepoMock.Object,
                _settingRepoMock.Object,
                _customerContextFactoryMock.Object,
                _mapperMock.Object);

            _request = new ValidateUserRequest("Email", "Password");

            var guid = Guid.NewGuid();
            var connectionString =
                    new Encryption(
                        Key,
                        Salt,
                        guid.ToByteArray())
                    .EncryptString("Connection String");

            _connection = new RootConnection(guid, connectionString);

            _userDto = new UserDto
            {
                UserGuid = Guid.NewGuid(),
                UserId = 1,
                Email = "Email Address",
                Name = "Name"
            };
        }

        [Test]
        public void ValidateUserRepositoryExceptionTest()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByEmail(It.IsAny<string>())).Throws<Exception>();

            var response = _authService.ValidateUser(_request);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.ServerError, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
            Assert.IsNull(response.Entity);
        }

        [Test]
        public void ValidateUserServiceExceptionNoConnectionFound()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByEmail(It.IsAny<string>())).Returns((RootConnection)null);

            var response = _authService.ValidateUser(_request);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.NotFound, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
            Assert.IsNull(response.Entity);
        }

        [Test]
        public void ValidateUserServiceExceptionEncryptionSaltSettingNotFound()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByEmail(It.IsAny<string>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns((string)null);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns(Key);

            var response = _authService.ValidateUser(_request);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.NotFound, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
            Assert.IsNull(response.Entity);
        }

        [Test]
        public void ValidateUserServiceExceptionEncryptionKeySettingNotFound()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByEmail(It.IsAny<string>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns(Salt);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns((string)null);

            var response = _authService.ValidateUser(_request);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.NotFound, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
            Assert.IsNull(response.Entity);
        }

        [Test]
        public void ValidateUserServiceExceptionUserNotFoundTest()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByEmail(It.IsAny<string>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns(Salt);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns(Key);
            _customerContextFactoryMock.Setup(x => x.CreateDbContext(It.IsAny<string>())).Returns(_customerDatabaseMock.Object);

            _request = new ValidateUserRequest("WrongEmail", "Password");

            var response = _authService.ValidateUser(_request);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.NotFound, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
            Assert.IsNull(response.Entity);
        }

        [Test]
        public void ValidateUserSuccessfulValidTest()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByEmail(It.IsAny<string>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns(Salt);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns(Key);
            _customerContextFactoryMock.Setup(x => x.CreateDbContext(It.IsAny<string>())).Returns(_customerDatabaseMock.Object);
            _mapperMock.Setup(x => x.Map<UserDto>(It.IsAny<User>())).Returns(_userDto);

            var response = _authService.ValidateUser(_request);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.Success, response.Code);
            Assert.IsEmpty(response.ErrorMessage);
            Assert.AreEqual(_userDto, response.Entity);
        }

        [Test]
        public void ValidateUserSuccessfulInvalidTest()
        {
            _rootRepoMock.Setup(x => x.FindConnectionByEmail(It.IsAny<string>())).Returns(_connection);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionSalt)).Returns(Salt);
            _settingRepoMock.Setup(x => x.FindSettingAsString(SettingName.EncryptionKey)).Returns(Key);
            _customerContextFactoryMock.Setup(x => x.CreateDbContext(It.IsAny<string>())).Returns(_customerDatabaseMock.Object);
            _mapperMock.Setup(x => x.Map<UserDto>(It.IsAny<User>())).Returns(_userDto);

            _request = new ValidateUserRequest("Email", "WrongPassword");

            var response = _authService.ValidateUser(_request);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.ValidationError, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
            Assert.IsNull(response.Entity);
        }
    }
}
