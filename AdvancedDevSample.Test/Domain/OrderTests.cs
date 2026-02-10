using System;
using System.Linq;
using AdvancedDevSample.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace AdvancedDevSample.Test.Domain
{
    public class OrderTests
    {
        [Fact]
        public void AddOrderItem_Should_AddItem_When_Valid()
        {
            // Arrange
            var order = new Order(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var unitPrice = 100m;
            var quantity = 2;

            // Act
            order.AddOrderItem(productId, unitPrice, quantity);

            // Assert
            order.Items.Should().HaveCount(1);
            var item = order.Items.First();
            item.ProductId.Should().Be(productId);
            item.Quantity.Should().Be(quantity);
            item.UnitPrice.Should().Be(unitPrice);
            order.TotalAmount.Should().Be(200m);
        }

        [Fact]
        public void AddOrderItem_Should_UpdateQuantity_When_ItemExists()
        {
            // Arrange
            var order = new Order(Guid.NewGuid());
            var productId = Guid.NewGuid();
            order.AddOrderItem(productId, 100m, 1);

            // Act
            order.AddOrderItem(productId, 100m, 2);

            // Assert
            order.Items.Should().HaveCount(1);
            var item = order.Items.First();
            item.Quantity.Should().Be(3);
            order.TotalAmount.Should().Be(300m);
        }

        [Fact]
        public void AddOrderItem_Should_ThrowArgumentException_When_QuantityIsZeroOrNegative()
        {
            // Arrange
            var order = new Order(Guid.NewGuid());

            // Act
            Action actZero = () => order.AddOrderItem(Guid.NewGuid(), 10m, 0);
            Action actNegative = () => order.AddOrderItem(Guid.NewGuid(), 10m, -1);

            // Assert
            actZero.Should().Throw<ArgumentException>()
                .WithMessage("*quantity*");
            actNegative.Should().Throw<ArgumentException>()
                .WithMessage("*quantity*");
        }

        [Fact]
        public void AddOrderItem_Should_ThrowArgumentException_When_UnitPriceIsNegative()
        {
            // Arrange
            var order = new Order(Guid.NewGuid());

            // Act
            Action act = () => order.AddOrderItem(Guid.NewGuid(), -10m, 1);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*UnitPrice*");
        }
    }
}
