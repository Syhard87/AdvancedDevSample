using System;
using System.Collections.Generic;
using AdvancedDevSample.Domain.Entities;


namespace AdvancedDevSample.Domain.Interfaces
{
    /// <summary>
    /// Contrat de persistance pour les produits.
    /// Méthodes synchrones pour être compatibles avec le code existant.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>Récupère un produit par son identifiant ou null si introuvable.</summary>
        Product? GetById(Guid id);

        /// <summary>Liste tous les produits.</summary>
        IEnumerable<Product> ListAll();

        /// <summary>Ajoute un produit.</summary>
        void Add(Product product);

        /// <summary>Supprime un produit par son identifiant.</summary>
        void Remove(Guid id);

        /// <summary>Sauvegarde / met à jour un produit existant.</summary>
        void Save(Product product);
    }
}
