using System;
using System.Collections.Generic;
using System.Threading.Tasks; // Nécessaire pour le mode asynchrone
using AdvancedDevSample.Domain.Entities;

namespace AdvancedDevSample.Domain.Interfaces
{
    public interface IProductRepository
    {
        // R (Read) - Récupérer tout
        Task<IEnumerable<Product>> GetAllAsync();

        // R (Read) - Récupérer un seul (peut renvoyer null si pas trouvé, d'où le "?")
        Task<Product?> GetByIdAsync(Guid id);

        // C (Create) - Ajouter
        Task AddAsync(Product product);

        // U (Update) - Mettre à jour
        Task UpdateAsync(Product product);

        // D (Delete) - Supprimer
        Task DeleteAsync(Guid id);
    }
}