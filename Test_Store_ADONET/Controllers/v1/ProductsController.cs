using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Test_Store_ADONET.DTOs;
using Test_Store_ADONET.Entities;
using Test_Store_ADONET.Exceptions;
using Test_Store_ADONET.Services.Products;

namespace Test_Store_ADONET.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    [EnableRateLimiting("token-jwt")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;
        private readonly ILogger<ProductsController> _logger;
        protected string ApiVersion => HttpContext.GetRequestedApiVersion().ToString();

        public ProductsController(IProductsService productsService, ILogger<ProductsController> logger)
        {
            _productsService = productsService;
            _logger = logger;
        }

        /// <summary>
        /// Handles GET requests to retrieve a list of products. Logs the request and any errors that occur during
        /// processing.
        /// </summary>
        /// <returns>Returns an Ok response with the list of products or a 500 error status if an exception occurs.</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                _logger.LogInformation($"Received a GET request to v{ApiVersion}/Products");
                var products = await _productsService.GetProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred during GET request to v{ApiVersion}/Products");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Handles GET requests to retrieve a product by its unique identifier.
        /// </summary>
        /// <param name="idProduct">The unique identifier used to locate the specific product in the database.</param>
        /// <returns>Returns an IActionResult indicating the success or failure of the request, along with the product data or an
        /// error message.</returns>
        [HttpGet("{idProduct}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetProductById(Guid idProduct)
        {
            try
            {
                _logger.LogInformation($"Received a GET request to v{ApiVersion}/Products/{idProduct}");
                Product product = await _productsService.GetProductById(idProduct);
                return Ok(product);
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex, $"BadRequestException occurred during GET request to v{ApiVersion}/Products/{idProduct}");
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"NotFoundException occurred during GET request to v{ApiVersion}/Products/{idProduct}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred during GET request to v{ApiVersion}/Products/{idProduct}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Handles the creation of a new product through a POST request.
        /// </summary>
        /// <param name="createProduct">Contains the details required to create a new product.</param>
        /// <returns>Returns a 201 status code with the newly created product or an error message based on the outcome.</returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO createProduct)
        {
            try
            {
                _logger.LogInformation($"Received a POST request to v{ApiVersion}/Products to create a new product");
                Product newProduct = await _productsService.CreateProduct(createProduct);
                return StatusCode(StatusCodes.Status201Created, newProduct);
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex, $"BadRequestException occurred during POST request to v{ApiVersion}/Products");
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"NotFoundException occurred during POST request to v{ApiVersion}/Products");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred during POST request to v{ApiVersion}/Products");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Handles HTTP PUT requests to update a product identified by a unique ID.
        /// </summary>
        /// <param name="idProduct">Specifies the unique identifier of the product to be updated.</param>
        /// <param name="updateProduct">Contains the new data for the product that needs to be updated.</param>
        /// <returns>Returns a status indicating the result of the update operation, such as NoContent, BadRequest, NotFound, or
        /// an error status.</returns>
        [HttpPut("{idProduct}")]
        [ProducesResponseType(200, Type = typeof(NoContentResult))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateProduct(Guid idProduct, [FromBody] CreateProductDTO updateProduct)
        {
            try
            {
                _logger.LogInformation($"Received a PUT request to v{ApiVersion}/Products/{idProduct} to update product");
                await _productsService.UpdateProduct(idProduct, updateProduct);
                return NoContent();
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex, $"BadRequestException occurred during PUT request to v{ApiVersion}/Products/{idProduct}");
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"NotFoundException occurred during PUT request to v{ApiVersion}/Products/{idProduct}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred during PUT request to v{ApiVersion}/Products/{idProduct}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Handles HTTP DELETE requests to remove a product identified by a unique identifier.
        /// </summary>
        /// <param name="idProduct">The unique identifier of the product to be deleted.</param>
        /// <returns>Returns a NoContent response if successful, or an appropriate error response based on the exception
        /// encountered.</returns>
        [HttpDelete("{idProduct}")]
        [ProducesResponseType(200, Type = typeof(NoContentResult))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteProduct(Guid idProduct)
        {
            try
            {
                _logger.LogInformation($"Received a DELETE request to v{ApiVersion}/Products/{idProduct}");
                await _productsService.deleteProduct(idProduct);
                return NoContent();
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex, $"BadRequestException occurred during DELETE request to v{ApiVersion}/Products/{idProduct}");
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"NotFoundException occurred during DELETE request to v{ApiVersion}/Products/{idProduct}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred during DELETE request to v{ApiVersion}/Products/{idProduct}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
