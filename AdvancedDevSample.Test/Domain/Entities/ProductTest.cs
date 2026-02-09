using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.ValueObjects;
using FluentAssertions; // C'est lui qui permet d'utiliser .Should()
using Xunit;

namespace AdvancedDevSample.Test.Domain.Entities
{
    public class ProductTest
    {
        [Fact]
        public void UpdatePrice_Should_Update_Price_When_Product_Is_Active()
        {
            // Arrange
            var initialPrice = new Price(10);
            var product = new Product("Produit Test", initialPrice);
            var newPrice = new Price(20);

            // Act
            // CORRECTION : On utilise UpdatePrice (le nom dans votre Product.cs)
            product.UpdatePrice(newPrice);

            // Assert
            // CORRECTION : On utilise la syntaxe FluentAssertions
            product.Price.Value.Should().Be(20);
        }

        [Fact]
        public void UpdatePrice_Should_Throw_Exception_When_Product_Is_Inactive()
        {
            // Arrange
            var product = new Product("Produit Test", new Price(10));

            // On désactive le produit "de force" pour le test
            product.Deactivate(); // Si vous avez ajouté cette méthode, sinon utilisez la Reflection ci-dessous :
            // typeof(Product).GetProperty(nameof(Product.IsActive))!.SetValue(product, false);

            var newPrice = new Price(20);

            // Act
            Action action = () => product.UpdatePrice(newPrice);

            // Assert
            action.Should().Throw<DomainException>()
                .WithMessage("Impossible de modifier un produit inactif.");
        }

        [Fact]
        public void ApplyDiscount_Should_Decrease_Price()
        {
            // Arrange
            var product = new Product("Produit Soldé", new Price(100));

            // Act
            product.ApplyDiscount(30);

            // Assert
            product.Price.Value.Should().Be(70);
        }
    }
}