using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        // --- 1. CREATE (Créer un produit) ---
        public async Task<Guid> CreateProductAsync(CreateProductRequest request)
        {
            // On convertit le decimal (DTO) en ValueObject (Domain)
            var price = new Price(request.Price);

            // On crée l'entité
            var product = new Product(request.Name, price);

            // On sauvegarde via le repository
            await _repo.AddAsync(product);

            return product.Id;
        }

        // --- 2. READ (Lire tous les produits) ---
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _repo.GetAllAsync();

            // On convertit les Entités en DTOs pour l'affichage
            return products.Select(p => new ProductDto(p.Id, p.Name, p.Price.Value));
        }

        // --- 3. READ (Lire un seul produit) ---
        public async Task<ProductDto?> GetProductByIdAsync(Guid id)
        {
            var product = await _repo.GetByIdAsync(id);

            if (product == null) return null;

            return new ProductDto(product.Id, product.Name, product.Price.Value);
        }

        // --- 4. UPDATE (Changer le prix) ---
        public async Task ChangePriceAsync(Guid id, decimal newPriceValue)
        {
            // A. On récupère le produit en base
            var product = await _repo.GetByIdAsync(id);

            if (product == null)
            {
                throw new KeyNotFoundException($"Le produit {id} n'existe pas.");
            }

            // B. CORRECTION : On convertit le decimal en objet Price
            var priceObject = new Price(newPriceValue);

            // C. On appelle la méthode de l'entité (UpdatePrice)
            product.UpdatePrice(priceObject);

            // D. On sauvegarde les modifications
            await _repo.UpdateAsync(product);
        }

        // --- 5. DELETE (Supprimer un produit) ---
        public async Task DeleteProductAsync(Guid id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}