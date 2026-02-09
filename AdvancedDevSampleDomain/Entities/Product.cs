using System;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Price Price { get; private set; }
        public bool IsActive { get; private set; }

        // --- AJOUT TECHNIQUE ---
        // Ce constructeur est requis par EF Core mais ne doit pas être utilisé par nous.
        // On désactive l'alerte "Non-nullable property" juste pour ces lignes.
#pragma warning disable CS8618
        protected Product() { }
#pragma warning restore CS8618

        // Constructeur principal (Celui qu'on utilise)
        public Product(string name, Price price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Le nom du produit est obligatoire.");

            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            IsActive = true;
        }

        // Constructeur complet (Pour reconstruire l'objet depuis la BDD)
        public Product(Guid id, string name, Price price, bool isActive)
        {
            Id = id;
            Name = name;
            Price = price;
            IsActive = isActive;
        }

        // Méthode de mise à jour du prix (Utilisée par ProductService)
        public void UpdatePrice(Price newPrice)
        {
            if (!IsActive)
                throw new DomainException("Impossible de modifier un produit inactif.");

            Price = newPrice;
        }

        // Méthode de rabais (Utilisée par les Tests)
        public void ApplyDiscount(decimal discountAmount)
        {
            if (!IsActive)
                throw new DomainException("Impossible de modifier un produit inactif.");

            // On calcule le nouveau montant
            decimal newAmount = Price.Value - discountAmount;

            // On recrée un objet Price (qui vérifiera tout seul si < 0)
            Price = new Price(newAmount);
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new DomainException("Le nom ne peut pas être vide.");
            Name = newName;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}