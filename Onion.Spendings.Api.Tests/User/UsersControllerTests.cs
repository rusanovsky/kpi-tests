using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using onion_spendings.User;
using Spendings.Core.User;
using Spendings.Orchrestrators.User;
using Xunit;

namespace Onion.Spendings.Api.Tests.User
{
    public class UsersControllerTests
    {
        private readonly UserController _realController;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserService> _userServiceMock;

        public UsersControllerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _userServiceMock = new Mock<IUserService>();

            _realController = new UserController(_mapperMock.Object, _userServiceMock.Object);
        }


        [Fact]
        public async Task PostAsync_IfServiceReturnsUser_ReturnsResponse()
        {
            // Arrange
            var user = new global::Spendings.Orchrestrators.User.Users
            {
                Login = "login",
                Password = "password"
            };

            var CoreUser = new global::Spendings.Core.User.Users
            {
                Id = 404,
                Login = "login",
                Password = "password"
            };
            var userFromService = new global::Spendings.Core.User.Users
            {
                Login = user.Login,
                Password = user.Password
            };

            var userAfterMapping = new global::Spendings.Core.User.Users
            {
                Login = userFromService.Login,
                Password = userFromService.Password
            };

            _userServiceMock.Setup(us => us.AddAsync(CoreUser))
                .ReturnsAsync(CoreUser);
            _mapperMock.Setup(m => m.Map<global::Spendings.Core.User.Users>(user))
                .Returns(userAfterMapping);
            _mapperMock.Setup(m => m.Map<global::Spendings.Orchrestrators.User.Users>(userAfterMapping))
                .Returns(user);

            // Act
            var result = await _realController.PostAsync(user) as OkObjectResult;

            // Assert
            result.StatusCode.Should().Be((int) HttpStatusCode.OK);
        }
    }
}