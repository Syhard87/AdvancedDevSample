using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedDevSample.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public decimal TotalAmount { get; private set; }
        
        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        // Constructor for EF Core
        private Order() { }

        public Order(Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            OrderDate = DateTime.UtcNow;
            TotalAmount = 0;
        }

        public void AddOrderItem(Guid productId, decimal unitPrice, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            if (unitPrice < 0)
                throw new ArgumentException("UnitPrice cannot be negative.", nameof(unitPrice));

            var existingItem = _items.SingleOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                // Optionally handle update quantity logic or throw exception
                // For this implementation, we'll increment quantity
                existingItem.AddQuantity(quantity);
            }
            else
            {
                var item = new OrderItem(Id, productId, quantity, unitPrice);
                _items.Add(item);
            }

            RecalculateTotal();
        }

        private void RecalculateTotal()
        {
            TotalAmount = _items.Sum(i => i.TotalLineAmount);
        }
    }
}
