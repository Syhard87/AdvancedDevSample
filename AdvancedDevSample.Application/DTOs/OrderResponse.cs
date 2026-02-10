using System;
using System.Collections.Generic;

namespace AdvancedDevSample.Application.DTOs
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderResponseItem> Items { get; set; } = new();
    }

    public class OrderResponseItem
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } // Added for better response, though not explicitly asked, it's good practice. Wait, user didn't ask for name. Let's stick to what's available in OrderItem or fetch it.
        // Actually, OrderItem only has ProductId. I won't fetch Product Name to keep it simple and strict to requirements unless I want to do a join.
        // Re-reading requirements: "Id, Date, Total, et liste détaillée".
        // Detailed list usually implies name, but I'll stick to what I have in OrderItem + Price/Quantity.
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalLineAmount { get; set; }
    }
}
