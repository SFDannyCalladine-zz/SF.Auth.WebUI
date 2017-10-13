using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using SF.Auth.Accounts;
using SF.Common.Domain.Exceptions;

namespace SF.Auth.UnitTests.Domain
{
    [TestFixture]
    public class UserTests
    {
        #region Private Fields

        private const string ForgottenPasswordGuid = "d95eb2f3-d9fb-4143-b046-f9cc34491f65";
        private ForgottenPassword _forgottenPassword;
        private User _user;

        #endregion Private Fields

        #region Public Methods

        [Test]
        public void AddPasswordRequestDomainValidationExceptionDeactivatedTest()
        {
            _forgottenPassword.Deactivate();

            Assert.Throws<DomainValidationException>(() =>
            {
                _user.AddPasswordResetRequest(_forgottenPassword, 5);
            });
        }

        [Test]
        public void AddPasswordRequestDomainValidationExceptionExpiredTest()
        {
            _forgottenPassword = new ForgottenPassword(new Guid(ForgottenPasswordGuid));
            Thread.Sleep(2000);

            Assert.Throws<DomainValidationException>(() =>
            {
                _user.AddPasswordResetRequest(_forgottenPassword, 0);
            });
        }

        [Test]
        public void AddPasswordRequestDomainValidationExceptionForgottenPasswordNullTest()
        {
            Assert.Throws<DomainValidationException>(() =>
            {
                _user.AddPasswordResetRequest(null, 5);
            });
        }

        [Test]
        public void AddPasswordRequestDomainValidationExceptionUsedTest()
        {
            _forgottenPassword.Use(5);

            Assert.Throws<DomainValidationException>(() =>
            {
                _user.AddPasswordResetRequest(_forgottenPassword, 5);
            });
        }

        [Test]
        public void AddPasswordRequestSuccessfulHasOtherActiveResetRequestsTest()
        {
            var firstRequest = new ForgottenPassword(new Guid(ForgottenPasswordGuid));
            _user.AddPasswordResetRequest(firstRequest, 5);
            _user.AddPasswordResetRequest(_forgottenPassword, 5);

            Assert.IsNotEmpty(_user.ForgottenPasswords);
            Assert.Contains(firstRequest, _user.ForgottenPasswords.ToList());
            Assert.Contains(_forgottenPassword, _user.ForgottenPasswords.ToList());
            Assert.AreEqual(true, firstRequest.Used);
        }

        [Test]
        public void AddPasswordRequestSuccessfulTest()
        {
            _user.AddPasswordResetRequest(_forgottenPassword, 5);

            Assert.IsNotEmpty(_user.ForgottenPasswords);
            Assert.Contains(_forgottenPassword, _user.ForgottenPasswords.ToList());
        }

        [Test]
        public void ConstructorEmailAddressDomainValidationExceptionEmptyTest()
        {
            Assert.Throws<DomainValidationException>(() =>
            {
                _user = new User(Guid.NewGuid(), "Name", string.Empty, "Password");
            });
        }

        [Test]
        public void ConstructorEmailAddressDomainValidationExceptionNullTest()
        {
            Assert.Throws<DomainValidationException>(() =>
            {
                _user = new User(Guid.NewGuid(), "Name", null, "Password");
            });
        }

        [Test]
        public void ConstructorNameDomainValidationExceptionEmptyTest()
        {
            Assert.Throws<DomainValidationException>(() =>
            {
                _user = new User(Guid.NewGuid(), string.Empty, "EmailAddress", "Password");
            });
        }

        [Test]
        public void ConstructorNameDomainValidationExceptionNullTest()
        {
            Assert.Throws<DomainValidationException>(() =>
            {
                _user = new User(Guid.NewGuid(), null, "EmailAddress", "Password");
            });
        }

        [Test]
        public void ConstructorPasswordDomainValidationExceptionEmptyTest()
        {
            Assert.Throws<DomainValidationException>(() =>
            {
                _user = new User(Guid.NewGuid(), "Name", "EmailAddress", string.Empty);
            });
        }

        [Test]
        public void ConstructorPasswordDomainValidationExceptionNullTest()
        {
            Assert.Throws<DomainValidationException>(() =>
            {
                _user = new User(Guid.NewGuid(), "Name", "EmailAddress", null);
            });
        }

        [Test]
        public void ConstructorSuccessfulTest()
        {
            var guid = Guid.NewGuid();

            var user = new User(
                guid,
                "Name",
                "EmailAddress",
                "Password");

            Assert.AreEqual(0, user.UserId);
            Assert.AreEqual(guid, user.UserGuid);
            Assert.AreEqual("Name", user.Name);
            Assert.AreEqual("EmailAddress", user.Email);
            Assert.IsTrue(user.ValidatePassword("Password"));

            Assert.IsNotNull(user.ForgottenPasswords);
            Assert.IsEmpty(user.ForgottenPasswords);
        }

        [Test]
        public void IsValidKeyFalseDeactivatedTest()
        {
            _user.AddPasswordResetRequest(_forgottenPassword, 5);
            _forgottenPassword.Deactivate();

            var result = _user.IsValidKey(_forgottenPassword.Key, 5);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsValidKeyFalseDoesNotExistTest()
        {
            var result = _user.IsValidKey(new Guid(ForgottenPasswordGuid), 5);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsValidKeyFalseExpiredTest()
        {
            _forgottenPassword = new ForgottenPassword(new Guid(ForgottenPasswordGuid));

            _user.AddPasswordResetRequest(_forgottenPassword, 5);
            Thread.Sleep(2000);

            var result = _user.IsValidKey(_forgottenPassword.Key, 0);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsValidKeyFalseUsedTest()
        {
            _user.AddPasswordResetRequest(_forgottenPassword, 5);
            _forgottenPassword.Use(5);

            var result = _user.IsValidKey(_forgottenPassword.Key, 5);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsValidKeyTrueTest()
        {
            _user.AddPasswordResetRequest(_forgottenPassword, 5);

            var result = _user.IsValidKey(_forgottenPassword.Key, 5);

            Assert.AreEqual(true, result);
        }

        [SetUp]
        public void SetUp()
        {
            _user = new User(Guid.NewGuid(), "Name", "EmailAddress", "Password");
            _forgottenPassword = new ForgottenPassword(new Guid(ForgottenPasswordGuid));
        }

        [Test]
        public void UpdatePasswordWithKeyDomainValidationExceptionDeactivatedTest()
        {
            _user.AddPasswordResetRequest(_forgottenPassword, 5);
            _forgottenPassword.Deactivate();

            Assert.Throws<DomainValidationException>(() =>
            {
                _user.UpdatePasswordWithKey(_forgottenPassword.Key, 5, "Password2");
            });

            var validateResult = _user.ValidatePassword("Password2");
            Assert.AreEqual(false, validateResult);
        }

        [Test]
        public void UpdatePasswordWithKeyDomainValidationExceptionDoesNotExistTest()
        {
            Assert.Throws<DomainValidationException>(() =>
            {
                _user.UpdatePasswordWithKey(new Guid(ForgottenPasswordGuid), 5, "Password2");
            });

            var validateResult = _user.ValidatePassword("Password2");
            Assert.AreEqual(false, validateResult);
        }

        [Test]
        public void UpdatePasswordWithKeyDomainValidationExceptionExpiredTest()
        {
            _forgottenPassword = new ForgottenPassword(new Guid(ForgottenPasswordGuid));

            _user.AddPasswordResetRequest(_forgottenPassword, 5);
            Thread.Sleep(2000);

            Assert.Throws<DomainValidationException>(() =>
            {
                _user.UpdatePasswordWithKey(_forgottenPassword.Key, 0, "Password2");
            });

            var validateResult = _user.ValidatePassword("Password2");
            Assert.AreEqual(false, validateResult);
        }

        [Test]
        public void UpdatePasswordWithKeyDomainValidationExceptionUsedTest()
        {
            _user.AddPasswordResetRequest(_forgottenPassword, 5);
            _forgottenPassword.Use(5);

            Assert.Throws<DomainValidationException>(() =>
            {
                _user.UpdatePasswordWithKey(_forgottenPassword.Key, 5, "Password2");
            });

            var validateResult = _user.ValidatePassword("Password2");
            Assert.AreEqual(false, validateResult);
        }

        [Test]
        public void UpdatePasswordWithKeySuccessfulTest()
        {
            _user.AddPasswordResetRequest(_forgottenPassword, 5);
            _user.UpdatePasswordWithKey(_forgottenPassword.Key, 5, "Password2");

            var result = _forgottenPassword.IsValid(5);
            var validateResult = _user.ValidatePassword("Password2");

            Assert.AreEqual(false, result);
            Assert.AreEqual(true, validateResult);
        }

        [TestCase("Password", "Password", true, TestName = "VerifyPasswordTestSuccessfulVerify")]
        [TestCase("Password1", "Password", false, TestName = "VerifyPasswordTestFailedVerify")]
        public void VerifyPasswordTest(
            string password,
            string passwordToTry,
            bool expectedResult)
        {
            _user = new User(
                Guid.NewGuid(),
                "Name",
                "EmailAddress",
                password);

            var result = _user.ValidatePassword(passwordToTry);

            Assert.AreEqual(expectedResult, result);
        }

        #endregion Public Methods
    }
}