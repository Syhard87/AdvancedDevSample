using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AdvancedDevSample.Api.Controllers;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AdvancedDevSample.Test.UnitTests
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _controller = new OrdersController(_orderServiceMock.Object);
        }

        private void SetupUserPrincipal(string userId)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [Fact]
        public async Task CreateOrder_Should_ReturnUnauthorized_When_UserClaimMissing()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() // No User
            };
            
            var request = new CreateOrderRequest();

            // Act
            var result = await _controller.CreateOrder(request);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task CreateOrder_Should_ReturnBadRequest_When_UserIdInvalid()
        {
            // Arrange
            SetupUserPrincipal("invalid-guid");
            var request = new CreateOrderRequest();

            // Act
            var result = await _controller.CreateOrder(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("Invalid User ID in token.");
        }

        [Fact]
        public async Task CreateOrder_Should_ReturnCreated_When_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            SetupUserPrincipal(userId.ToString());
            
            var request = new CreateOrderRequest();
            var response = new OrderResponse { Id = Guid.NewGuid() };

            _orderServiceMock.Setup(s => s.CreateOrderAsync(userId, request))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.CreateOrder(request);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.StatusCode.Should().Be(201);
            createdResult.Value.Should().Be(response);
            createdResult.ActionName.Should().Be("CreateOrder"); // As implemented in controller
        }

        [Fact]
        public async Task CreateOrder_Should_ReturnNotFound_When_ProductNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            SetupUserPrincipal(userId.ToString());
            var request = new CreateOrderRequest();

            _orderServiceMock.Setup(s => s.CreateOrderAsync(userId, request))
                .ThrowsAsync(new Exception("Product with ID X not found."));

            // Act
            var result = await _controller.CreateOrder(request);

            // Assert
            var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.StatusCode.Should().Be(404);
            // Verify message content if needed, e.g. using reflection on anonymous type or just checking type
        }

        [Fact]
        public async Task CreateOrder_Should_ReturnBadRequest_When_OtherException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            SetupUserPrincipal(userId.ToString());
            var request = new CreateOrderRequest();

            _orderServiceMock.Setup(s => s.CreateOrderAsync(userId, request))
                .ThrowsAsync(new Exception("Some other error"));

            // Act
            var result = await _controller.CreateOrder(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
