using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using Test_Store_ADONET.DTOs;
using Test_Store_ADONET.Entities;
using Test_Store_ADONET.Exceptions;
using Test_Store_ADONET.Services.Brands;

namespace Test_Store_ADONET.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    [EnableRateLimiting("token-jwt")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandsService _brandsService;
        private readonly ILogger<BrandsController> _logger;
        protected string ApiVersion => HttpContext.GetRequestedApiVersion().ToString();
        public BrandsController(IBrandsService brandsService, ILogger<BrandsController> logger)
        {
            _brandsService = brandsService;
            _logger = logger;
        }

        /// <summary>
        /// Handles GET requests to retrieve a list of brands. Logs the request and any errors that occur during
        /// processing.
        /// </summary>
        /// <returns>Returns an Ok response with the list of brands or a 500 error status if an exception occurs.</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Brand>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBrands()
        {
            try
            {
                _logger.LogInformation($"Received a GET request to v{ApiVersion}/Brands.");
                var brands = await _brandsService.GetBrands();
                return Ok(brands);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred during GET request to v{ApiVersion}/Brands.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Handles GET requests to retrieve a brand by its unique identifier.
        /// </summary>
        /// <param name="idBrand">The unique identifier used to locate a specific brand in the system.</param>
        /// <returns>Returns an IActionResult indicating the success or failure of the request, along with the brand data if
        /// successful.</returns>
        [HttpGet("{idBrand}")]
        [ProducesResponseType(200, Type = typeof(Brand))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBrandById(Guid idBrand)
        {
            try
            {
                _logger.LogInformation($"Received a GET request to v{ApiVersion}/Brands/{idBrand}.");
                Brand brand = await _brandsService.GetBrandById(idBrand);
                return Ok(brand);
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex, $"Bad request error during GET request to v{ApiVersion}/Brands/{idBrand}.");
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Brand not found with id {idBrand} during GET request to v{ApiVersion}/Brands/{idBrand}.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred during GET request to v{ApiVersion}/Brands/{idBrand}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Handles the creation of a new brand through a POST request.
        /// </summary>
        /// <param name="createBrand">Contains the necessary data to create a new brand.</param>
        /// <returns>Returns a 201 status code with the newly created brand or a 500 status code in case of an error.</returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Brand))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateBrand([FromBody] CreateBrandDTO createBrand)
        {
            try
            {
                _logger.LogInformation($"Received a POST request to v{ApiVersion}/Brands to create a new brand.");
                Brand newBrand = await _brandsService.CreateBrand(createBrand);
                return StatusCode(StatusCodes.Status201Created, newBrand);
             }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred during POST request to v{ApiVersion}/Brands to create a new brand.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Handles the update of a brand identified by a unique identifier.
        /// </summary>
        /// <param name="idBrand">Specifies the unique identifier of the brand to be updated.</param>
        /// <param name="updateBrand">Contains the new data for the brand that needs to be updated.</param>
        /// <returns>Returns a status indicating the result of the update operation.</returns>
        [HttpPut("{idBrand}")]
        [ProducesResponseType(200, Type = typeof(NoContentResult))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateBrand(Guid idBrand, [FromBody] CreateBrandDTO updateBrand)
        {
            try
            {
                _logger.LogInformation($"Received a PUT request to v{ApiVersion}/Brands/{idBrand} to update brand.");
                await _brandsService.UpdateBrand(idBrand, updateBrand);
                return NoContent();
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex, $"Bad request error during PUT request to v{ApiVersion}/Brands/{idBrand}.");
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Brand not found with id {idBrand} during PUT request to v{ApiVersion}/Brands/{idBrand}.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred during PUT request to v{ApiVersion}/Brands/{idBrand}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Handles the deletion of a brand identified by a unique identifier.
        /// </summary>
        /// <param name="idBrand">The unique identifier for the brand to be deleted.</param>
        /// <returns>Returns a NoContent response if successful, or an appropriate error response if an exception occurs.</returns>
        [HttpDelete("{idBrand}")]
        [ProducesResponseType(200, Type = typeof(NoContentResult))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteBrand(Guid idBrand)
        {
            try
            {
                _logger.LogInformation($"Received a DELETE request to v{ApiVersion}/Brands/{idBrand}.");
                await _brandsService.deleteBrand(idBrand);
                return NoContent();
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex, $"Bad request error during DELETE request to v{ApiVersion}/Brands/{idBrand}.");
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Brand not found with id {idBrand} during DELETE request to v{ApiVersion}/Brands/{idBrand}.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred during DELETE request to v{ApiVersion}/Brands/{idBrand}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
