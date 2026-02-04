using AdvancedDevSample.Domain.Exception;
using Microsoft.AspNetCore.Mvc;
using AdvancedDevSample.Domain.Services;

namespace AdvancedDevSample.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPut("{id}/price")]
        public IActionResult ChangePrice(Guid id, [FromBody] ChangePriceRequest request)
        {
            try
            {
                _productService.ChangePrice(id, request);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DomaineException ex)
            {
                return badRequest(ex.Message);
            }
        }
    }
}
