using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Exceptions;

namespace AdvancedDevSample.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    [Authorize] // <--- CORRECTIF : On sécurise TOUTE la classe (y compris le GET) pour le test
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        // --- 1. GET ALL ---
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        // --- 2. GET BY ID ---
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound($"Le produit {id} n'existe pas.");
            }

            return Ok(product);
        }

        // --- 3. POST ---
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            try
            {
                var createdId = await _productService.CreateProductAsync(request);
                // Renvoie 201 Created
                return CreatedAtAction(nameof(GetById), new { id = createdId }, new { id = createdId });
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // --- 4. PUT ---
        [HttpPut("{id}/price")]
        public async Task<IActionResult> ChangePrice(Guid id, [FromBody] ChangePriceRequest request)
        {
            try
            {
                await _productService.ChangePriceAsync(id, request.NewPrice);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // --- 5. DELETE ---
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}