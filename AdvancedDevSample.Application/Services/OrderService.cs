using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Interfaces;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace AdvancedDevSample.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<OrderResponse> CreateOrderAsync(Guid userId, CreateOrderRequest request)
        {
            _logger.LogInformation("Creating order for user {UserId} with {ItemCount} items.", userId, request.Items?.Count ?? 0);

            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));

            if (request == null || request.Items == null || !request.Items.Any())
                throw new ArgumentException("Order must contain at least one item.", nameof(request));

            var order = new Order(userId);

            foreach (var itemDto in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null)
                {
                    _logger.LogWarning("Product {ProductId} not found.", itemDto.ProductId);
                    throw new Exception($"Product with ID {itemDto.ProductId} not found.");
                }

                // Assuming Product entity has a Price property that is a Value Object or decimal.
                // Based on previous context (ApplicationDbContext): .OwnsOne(p => p.Price)
                // So product.Price.Value should be the decimal value.
                order.AddOrderItem(product.Id, product.Price.Value, itemDto.Quantity);
            }

            await _orderRepository.AddAsync(order);

            _logger.LogInformation("Order {OrderId} created successfully with total {TotalAmount}.", order.Id, order.TotalAmount);

            return MapToResponse(order);
        }

        private OrderResponse MapToResponse(Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Items = order.Items.Select(i => new OrderResponseItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalLineAmount = i.TotalLineAmount
                }).ToList()
            };
        }
    }
}
