using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using Xunit;

namespace AdvancedDevSample.Test.UnitTests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _productService = new ProductService(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task GetProductByIdAsync_Should_ReturnProduct_When_Exists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product("Test Product", new Price(100m));
            // Reflection to set Id as it is private set and generated in ctor
            typeof(Product).GetProperty("Id").SetValue(product, productId);

            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(productId);
            result.Name.Should().Be("Test Product");
            result.Price.Should().Be(100m);
        }

        [Fact]
        public async Task GetProductByIdAsync_Should_ReturnNull_When_NotExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllProductsAsync_Should_ReturnList()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product("P1", new Price(10)),
                new Product("P2", new Price(20))
            };
            _productRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            result.Should().HaveCount(2);
            result.First().Name.Should().Be("P1");
        }

        [Fact]
        public async Task CreateProductAsync_Should_CallRepository()
        {
            // Arrange
            var request = new CreateProductRequest
            {
                Name = "New Product",
                Price = 50m
            };

            // Act
            var result = await _productService.CreateProductAsync(request);

            // Assert
            result.Should().NotBeEmpty();
            _productRepositoryMock.Verify(r => r.AddAsync(It.Is<Product>(p => 
                p.Name == "New Product" && p.Price.Value == 50m
            )), Times.Once);
        }
    }
}
