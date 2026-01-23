using System;
using System.Collections.Generic;
using System.Text;
using AdvancedDevSample.Domain.Execptions;

namespace AdvancedDevSample.Domain.Entities
{
    public class Product
    {
        /// <summary>
        /// Représente un produit vendable.
        /// </summary>
        
        public Guid Id { get; private set; } // Identité
        public Decimal Price { get; private set; }
        public bool isActive { get; private set; } // false par défaut

        public Product() { 
            this.isActive = true;
        }

        public void Changeprice(decimal newPrice)
        {
            // Règle métier : le prix ne peut pas être inférieur à zéro.
            if (newPrice <= 0)
            {
                throw new DomainExeception("Le prix ne peut pas être inférieur à zéro.");
            }

            // Règle métier :le produit ne doit pas être inactif
            if (!isActive)
            {
                throw new DomainExeception("Le produit est inactif.");
            }

            //seul endroit ou on change le prix. 
            this.Price = newPrice;
        }
    }
}
