using System;
using System.ComponentModel.DataAnnotations;

namespace AdvancedDevSample.Application.DTOs
{
    // 1. Celui que vous aviez déjà (pour changer le prix)
    public class ChangePriceRequest
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être supérieur à 0.")]
        public decimal NewPrice { get; set; }
    }

    // 2. AJOUT : Celui pour créer un produit (Requis par ProductService)
    public class CreateProductRequest
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être positif")]
        public decimal Price { get; set; }
    }

    // 3. AJOUT : Celui pour afficher le produit (Requis par ProductService et Controller)
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // Constructeur simple pour faciliter la création
        public ProductDto(Guid id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        // Constructeur vide nécessaire parfois pour la sérialisation JSON
        public ProductDto() { }
    }
}