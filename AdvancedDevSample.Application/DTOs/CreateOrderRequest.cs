using System;
using System.Collections.Generic;

namespace AdvancedDevSample.Application.DTOs
{
    public class CreateOrderRequest
    {
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
