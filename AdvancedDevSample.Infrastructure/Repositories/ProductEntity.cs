using System;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; // Le nom est obligatoire
        public decimal Price { get; set; }
        public bool IsActive { get; set; }

        public ProductEntity() { }

        public ProductEntity(Guid id, string name, decimal price, bool isActive)
        {
            Id = id;
            Name = name;
            Price = price;
            IsActive = isActive;
        }

        // C'est ICI que l'erreur se produisait
        public Product ToDomain()
        {
            // On utilise le bon ordre : Id, Name, Price, IsActive
            return new Product(
                Id,
                Name,
                new Price(Price),
                IsActive
            );
        }

        public static ProductEntity FromDomain(Product product)
        {
            return new ProductEntity(
                product.Id,
                product.Name,
                product.Price.Value,
                product.IsActive
            );
        }
    }
}