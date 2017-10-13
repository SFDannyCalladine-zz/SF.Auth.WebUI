using NUnit.Framework;
using SF.Auth.Accounts;
using SF.Common.Domain.Exceptions;
using System;
using System.Threading;

namespace SF.Auth.UnitTests.Domain
{
    [TestFixture]
    public class ForgottenPasswordTests
    {
        #region Private Fields

        private ForgottenPassword _forgottenPassword;

        private const string ForgottenPasswordGuid = "d95eb2f3-d9fb-4143-b046-f9cc34491f65";

        #endregion Private Fields

        #region Public Methods

        [Test]
        public void ConstructorSuccessfulTest()
        {
            _forgottenPassword = new ForgottenPassword(new Guid(ForgottenPasswordGuid));

            Assert.IsNotNull(_forgottenPassword.Key);
            Assert.GreaterOrEqual(DateTime.Now, _forgottenPassword.Created);
            Assert.AreEqual(false, _forgottenPassword.Used);
        }

        [Test]
        public void IsExpiredFalse()
        {
            _forgottenPassword = new ForgottenPassword(new Guid(ForgottenPasswordGuid));

            var result = _forgottenPassword.IsExpired(2);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsExpiredTrue()
        {
            _forgottenPassword = new ForgottenPassword(new Guid(ForgottenPasswordGuid));

            Thread.Sleep(2000);

            var result = _forgottenPassword.IsExpired(0);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void IsValidFalseAlreadyUsedTest()
        {
            _forgottenPassword.Use(5);
            var result = _forgottenPassword.IsValid(5);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsValidFalseDeactivatedTest()
        {
            _forgottenPassword.Deactivate();
            var result = _forgottenPassword.IsValid(5);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsValidFalseIsExpiredTest()
        {
            _forgottenPassword = new ForgottenPassword(new Guid(ForgottenPasswordGuid));
            Thread.Sleep(2000);

            var result = _forgottenPassword.IsValid(0);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsValidTrueTest()
        {
            var result = _forgottenPassword.IsValid(5);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void DeactivateTest()
        {
            _forgottenPassword.Deactivate();

            Assert.AreEqual(true, _forgottenPassword.Used);
        }

        [SetUp]
        public void SetUp()
        {
            _forgottenPassword = new ForgottenPassword(new Guid(ForgottenPasswordGuid));
        }

        [Test]
        public void UseSuccessfulTest()
        {
            _forgottenPassword.Use(5);

            Assert.AreEqual(true, _forgottenPassword.Used);
        }

        [Test]
        public void UseDomainValidationExceptionAlreadyUsedTest()
        {
            _forgottenPassword.Use(5);

            Assert.Throws<DomainValidationException>(() =>
            {
                _forgottenPassword.Use(5);
            });
        }

        [Test]
        public void UseDomainValidationExceptionExpiredTest()
        {
            _forgottenPassword = new ForgottenPassword(new Guid(ForgottenPasswordGuid));
            Thread.Sleep(2000);

            Assert.Throws<DomainValidationException>(() =>
            {
                _forgottenPassword.Use(0);
            });
        }

        [Test]
        public void UseDomainValidationExceptionHasBeenDeactivatedTest()
        {
            _forgottenPassword.Deactivate();

            Assert.Throws<DomainValidationException>(() =>
            {
                _forgottenPassword.Use(5);
            });
        }

        #endregion Public Methods
    }
}
