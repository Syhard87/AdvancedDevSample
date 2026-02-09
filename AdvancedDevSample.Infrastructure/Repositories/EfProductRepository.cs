using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    public class InMemoryProductRepository : IProductRepository
    {
        // Ceci simule ta table SQL en mémoire vive.
        // "static" permet de garder les données tant que l'API tourne.
        private static readonly List<Product> _database = new();

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult((IEnumerable<Product>)_database);
        }

        public Task<Product?> GetByIdAsync(Guid id)
        {
            var product = _database.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(product);
        }

        public Task AddAsync(Product product)
        {
            _database.Add(product);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Product product)
        {
            // Dans une liste en mémoire, l'objet est déjà modifié par référence.
            // Dans une vraie BDD SQL, on ferait _context.Products.Update(product);

            // On vérifie juste qu'il existe
            var existingProduct = _database.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Le produit {product.Id} n'existe pas.");
            }

            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            var product = _database.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _database.Remove(product);
            }
            return Task.CompletedTask;
        }
    }
}