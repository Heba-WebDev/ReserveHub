using Service.Contracts;
using FakeItEasy;
using Presentation.Controllers;
using Shared.DataTransferObjects.Auth;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Shared.DataTransferObjects;

namespace Presentation.Test.Controllers
{
    public class AuthControllerTest
    {
        private readonly IServiceManager _services;
        private readonly AuthController _authController;

        public AuthControllerTest()
        {
            _services = A.Fake<IServiceManager>();
            _authController = new AuthController(_services);
        }

        [Fact]
        public async void AuthController_CreateUser_ReturnOk()
        {
            // Arrange
            var registerDto = A.Fake<RegisterDto>();
            var expectedResult = A.Fake<BasedResponseDto>();
            A.CallTo(() => _services.AuthService.Register(registerDto))
                .Returns(expectedResult);

            // Act
            var result = await _authController.CreateUser(registerDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().Be(expectedResult);
        }

        [Fact]
        public async void AuthController_Login_ReturnOk()
        {
            // Arrange
            var loginDto = A.Fake<LoginDto>();
            var expectedResult = A.Fake<BasedResponseDto>();
            A.CallTo(() => _services.UserService.ValidateUser(loginDto)).Returns(Task.FromResult(true));
            A.CallTo(() => _services.AuthService.Login(loginDto, true)).Returns(Task.FromResult(expectedResult));

            // Act
            var result = await _authController.Login(loginDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async void AuthController_Login_ReturnUnauthorized()
        {
            // Arrange
            var loginDto = A.Fake<LoginDto>();
            A.CallTo(() => _services.UserService.ValidateUser(loginDto)).Returns(Task.FromResult(false));

            // Act
            var result = await _authController.Login(loginDto);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async void AuthController_Refresh_ReturnsOk()
        {
            // Arrange
            var tokenDto = A.Fake<TokenDto>();
            A.CallTo(() => _services.AuthService.RefreshToken(tokenDto)).Returns(Task.FromResult(A.Fake<BasedResponseDto>()));

            // Set
            var result = await _authController.Refresh(tokenDto);

            // Assert
            result.Should().NotBeNull();
            result.As<OkObjectResult>().StatusCode.Should().Be(200);
        }
    }
}
