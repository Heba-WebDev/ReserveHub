

using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Presentation.Controllers;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Presentation.Test.Controllers
{
    public class CustomersControllerTest
    {
        private readonly IServiceManager _services;
        private readonly CustomersController _customersController;

        public CustomersControllerTest()
        {
            _services = A.Fake<IServiceManager>();
            _customersController = new CustomersController(_services);
        }

        [Fact]
        public async void CustomersController_CreateCustomer_ReturnOk()
        {
            // Arrange
            var dto = A.Fake<CreateCustomerRequestDto>();
            A.CallTo(() => _services.CustomerService.CreateCustomer(dto)).Returns(A.Fake<CustomersDto>());

            // Act
            var result = await _customersController.CreateCustomer(dto);

            // Assert
            result.Should().BeOfType<CreatedAtRouteResult>();
            result.As<CreatedAtRouteResult>().StatusCode.Should().Be(201);
        }
    }
}