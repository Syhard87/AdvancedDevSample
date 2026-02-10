using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdvancedDevSample.Api.Controllers;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AdvancedDevSample.Test.UnitTests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly ProductService _productService;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _productService = new ProductService(_productRepositoryMock.Object);
            _controller = new ProductsController(_productService);
        }

        [Fact]
        public async Task GetAll_Should_ReturnOk_WithList()
        {
            // Arrange
            var products = new List<Product> { new Product("P1", new Price(10)) };
            _productRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            var returnedList = okResult.Value.Should().BeAssignableTo<IEnumerable<ProductDto>>().Subject;
            returnedList.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetById_Should_ReturnOk_WhenFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var product = new Product("P1", new Price(10));
             // Reflection to set Id
            typeof(Product).GetProperty("Id").SetValue(product, id);

            _productRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            ((ProductDto)okResult.Value).Id.Should().Be(id);
        }

        [Fact]
        public async Task GetById_Should_ReturnNotFound_WhenNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _productRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Create_Should_ReturnCreated()
        {
            // Arrange
            var request = new CreateProductRequest { Name = "New", Price = 10 };

            // Act
            var result = await _controller.Create(request);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.StatusCode.Should().Be(201);
        }
    }
}
