using System;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Domain.Entities
{
    public class Product
    {
        private Guid guid;

        /// <summary>
        /// Représente un produit vendable.
        /// </summary>
        public Guid Id { get; private set; } // Identité
        public Price Price { get; private set; } // Invariant encapsulé dans Price
        public bool IsActive { get; private set; } // true par défaut

        public Product(Price price) : this(Guid.NewGuid(), price) { }

        public Product(Guid id, Price price,bool IsActive)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Price = price; // Price valide par construction
            IsActive = true;
        }

        // Constructeur requis par certains ORMs ; protégé pour empêcher l'utilisation publique.
        protected Product()
        {
            IsActive = true;
        }

        public Product(Guid guid, Price price)
        {
            this.guid = guid;
            Price = price;
        }

        public void ChangePrice(Price newPrice)
        {
            // Règle métier : le produit ne doit pas être inactif
            if (!IsActive)
            {
                throw new DomainException("Le produit est inactif.");
            }

            // Invariant déjà garanti par Price
            Price = newPrice;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;

        public void ChangePrice(decimal newPriceValue)
        {
            throw new NotImplementedException();
        }
    }
}