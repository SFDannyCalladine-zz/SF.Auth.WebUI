using AutoMapper;
using Moq;
using NUnit.Framework;
using SF.Auth.DataTransferObjects.Help;
using SF.Auth.Repositories.Interfaces;
using SF.Auth.Services;
using SF.Common.Help;
using SF.Common.ServiceModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SF.Auth.UnitTests.Services
{
    [TestFixture]
    public class HelpServiceTests
    {
        private Mock<IHelpRepository> _helpRepoMock;

        private Mock<IMapper> _mapperMock;

        private HelpService _helpService;

        [SetUp]
        public void SetUp()
        {
            _helpRepoMock = new Mock<IHelpRepository>();
            _mapperMock = new Mock<IMapper>();

            _helpService = new HelpService(
                _helpRepoMock.Object,
                _mapperMock.Object);
        }

        [Test]
        public void GetAllLinksSuccessfulNoneTest()
        {
            var helpLinks = new List<HelpLink>();

            _helpRepoMock.Setup(x => x.GetAllLinks()).Returns(helpLinks);
            _mapperMock.Setup(x => x.Map<IList<HelpLinkDto>>(helpLinks)).Returns(new List<HelpLinkDto>());

            var response = _helpService.GetAllLinks();

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.Success, response.Code);
            Assert.IsEmpty(response.ErrorMessage);
            Assert.IsNotNull(response.Entity);
            Assert.IsEmpty(response.Entity);
        }

        [Test]
        public void GetAllLinksSuccessfulSingleTest()
        {
            var helpLink = new HelpLink("Url", "Text", 1);
            var helpLinks = new List<HelpLink> { helpLink };

            var helpLinkDto = new HelpLinkDto
            {
                Url = "Url",
                LinkText = "Text",
                Order = 1
            };

            var helpLinkDtos = new List<HelpLinkDto> { helpLinkDto };

            _helpRepoMock.Setup(x => x.GetAllLinks()).Returns(helpLinks);
            _mapperMock.Setup(x => x.Map<IList<HelpLinkDto>>(helpLinks)).Returns(helpLinkDtos);

            var response = _helpService.GetAllLinks();

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.Success, response.Code);
            Assert.IsEmpty(response.ErrorMessage);
            Assert.IsNotNull(response.Entity);
            Assert.AreEqual(1, response.Entity.Count);
            Assert.Contains(helpLinkDto, response.Entity.ToList());
        }

        [Test]
        public void GetAllLinksSuccessfulMultipleTest()
        {
            var helpLink1 = new HelpLink("Url1", "Text1", 1);
            var helpLink2 = new HelpLink("Url2", "Text2", 2);
            var helpLinks = new List<HelpLink> { helpLink1, helpLink2 };

            var helpLinkDto1 = new HelpLinkDto
            {
                Url = "Url1",
                LinkText = "Text1",
                Order = 1
            };

            var helpLinkDto2 = new HelpLinkDto
            {
                Url = "Url2",
                LinkText = "Text2",
                Order = 2
            };

            var helpLinkDtos = new List<HelpLinkDto> { helpLinkDto1, helpLinkDto2 };

            _helpRepoMock.Setup(x => x.GetAllLinks()).Returns(helpLinks);
            _mapperMock.Setup(x => x.Map<IList<HelpLinkDto>>(helpLinks)).Returns(helpLinkDtos);

            var response = _helpService.GetAllLinks();

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.Success, response.Code);
            Assert.IsEmpty(response.ErrorMessage);
            Assert.IsNotNull(response.Entity);
            Assert.AreEqual(2, response.Entity.Count);
            Assert.Contains(helpLinkDto1, response.Entity.ToList());
            Assert.Contains(helpLinkDto2, response.Entity.ToList());
        }

        [Test]
        public void GetAllLinksExceptionTest()
        {
            _helpRepoMock.Setup(x => x.GetAllLinks()).Throws<Exception>();

            var response = _helpService.GetAllLinks();

            _mapperMock.Verify(x => x.Map<IList<HelpLinkDto>>(It.IsAny<IList<HelpLink>>()), Times.Never);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.ServerError, response.Code);
            Assert.IsNotEmpty(response.ErrorMessage);
            Assert.IsNull(response.Entity);
        }
    }
}
