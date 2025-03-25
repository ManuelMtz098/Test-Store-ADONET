using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using Test_Store_ADONET.DTOs;
using Test_Store_ADONET.Entities;
using Test_Store_ADONET.Exceptions;
using Test_Store_ADONET.Services.Login;

namespace Test_Store_ADONET.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [EnableRateLimiting("fixed-by-ip")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly ILogger<LoginController> _logger;
        protected string ApiVersion => HttpContext.GetRequestedApiVersion().ToString();
        public LoginController(ILoginService loginService, ILogger<LoginController> logger)
        {
            _loginService = loginService;
            _logger = logger;
        }

        /// <summary>
        /// Handles user login requests and returns a token upon successful authentication.
        /// </summary>
        /// <param name="login">Contains the user's credentials required for authentication.</param>
        /// <returns>Returns a token for the user upon successful login or an error message if the login fails.</returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(UserLoginWithToken))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            try
            {
                _logger.LogInformation(message: $"Received a POST request to v{ApiVersion}/Login for user {login.Username}");

                UserLoginWithToken userLogin = await _loginService.Login(login);

                _logger.LogInformation($"Login successful for user {login.Username}.");

                return Ok(userLogin);
            }
            catch (BadRequestException ex)
            {
                _logger.LogError(ex, $"An error occurred during login at v{ApiVersion}/Login for user {login.Username}");
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, $"An error occurred during login at v{ApiVersion}/Login for user {login.Username}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred during login at v{ApiVersion}/Login for user {login.Username}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

    }
}
