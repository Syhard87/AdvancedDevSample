using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;

namespace AdvancedDevSample.Test.UnitTests
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ILogger<OrderService>> _loggerMock;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _loggerMock = new Mock<ILogger<OrderService>>();

            _orderService = new OrderService(
                _orderRepositoryMock.Object,
                _productRepositoryMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldCreateOrder_WhenProductExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var quantity = 2;
            var unitPrice = 100m;
            
            var request = new CreateOrderRequest
            {
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto { ProductId = productId, Quantity = quantity }
                }
            };

            // Setup Product with Price
            var price = new Price(unitPrice);
            // Using the full constructor to simulate an existing product from DB
            var product = new Product(productId, "Test Product", price, true);

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(product);

            // Act
            var result = await _orderService.CreateOrderAsync(userId, request);

            // Assert
            Assert.NotNull(result);
            // Asserting result properties
            Assert.Single(result.Items);
            Assert.Equal(productId, result.Items[0].ProductId);
            Assert.Equal(quantity, result.Items[0].Quantity);
            Assert.Equal(unitPrice, result.Items[0].UnitPrice);
            Assert.Equal(quantity * unitPrice, result.TotalAmount);

            // Verify Repository was called
            _orderRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Order>(o => 
                o.UserId == userId && 
                o.Items.Count == 1 &&
                o.TotalAmount == quantity * unitPrice
            )), Times.Once);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldThrowException_WhenProductDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var request = new CreateOrderRequest
            {
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto { ProductId = productId, Quantity = 1 }
                }
            };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync((Product)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => 
                _orderService.CreateOrderAsync(userId, request));
            
            Assert.Contains($"Product with ID {productId} not found", exception.Message);
            
            _orderRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Order>()), Times.Never);
        }
        [Fact]
        public async Task CreateOrderAsync_ShouldThrowArgumentException_When_UserIdIsEmpty()
        {
            // Arrange
            var userId = Guid.Empty;
            var request = new CreateOrderRequest
            {
                Items = new List<OrderItemDto> { new OrderItemDto { ProductId = Guid.NewGuid(), Quantity = 1 } }
            };

            // Act
            Func<Task> act = async () => await _orderService.CreateOrderAsync(userId, request);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*User ID*");
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldThrowArgumentException_When_ItemsIsEmpty()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new CreateOrderRequest { Items = new List<OrderItemDto>() };

            // Act
            Func<Task> act = async () => await _orderService.CreateOrderAsync(userId, request);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Order must contain at least one item*");
        }

        [Fact]
        public async Task CreateOrderAsync_Should_LogWarning_When_ProductNotFound()
        {
             // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var request = new CreateOrderRequest
            {
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto { ProductId = productId, Quantity = 1 }
                }
            };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync((Product)null);

            // Act
            try
            {
                await _orderService.CreateOrderAsync(userId, request);
            }
            catch
            {
                // Ignore exception, we are checking logger
            }

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Product {productId} not found")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
