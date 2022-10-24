using HotelListing.Identity.DTOs;
using HotelListing.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthService authService, ILogger<AccountController> logger)
        {
            _authService = authService;
            _logger = logger;
        }


        // POST: api/Account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register([FromBody] UserDTO userDTO)
        {
            try
            {
                _logger.LogInformation("Registration attempt by '{Email}'", userDTO.Email);

                IEnumerable<IdentityError> errors = await _authService.RegisterUser(userDTO);
                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    _logger.LogInformation("Registration attempt by '{Email}' generated a 'BadRequest' : '{ModelState}'", userDTO.Email, ModelState);
                    return BadRequest(ModelState);
                }
                _logger.LogInformation("Registration attempt by '{Email}' was successful", userDTO.Email);
                return Ok();
            }
            catch (Exception exc)
            {
                _logger.LogError("Registration attempt by '{userDTO.Email}' failed", userDTO.Email);
                _logger.LogError("Exception Message: {Message}", exc.Message);
                _logger.LogError("Exception StackTrace: {StackTrace}", exc.StackTrace);
                return Problem("An unexpected error occurred on our end during the registration process.", statusCode: 500);
            }
        }

        // POST: api/Account/login
        [HttpPost] 
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                _logger.LogInformation("Login attempt by '{Email}'", loginDTO.Email);

                TokenDTO? tokenDTO = await _authService.Login(loginDTO);
                if (tokenDTO == null)
                {
                    _logger.LogInformation("Login attempt failed because '{Email}' is 'unauthorized'", loginDTO.Email);
                    return Unauthorized();
                }
                _logger.LogInformation("Login attempt by '{Email}' was successful.", loginDTO.Email);
                return Ok(tokenDTO);
            }
            catch (Exception exc)
            {
                _logger.LogError("Login attempt by '{loginDTO.Email}' failed", loginDTO.Email);
                _logger.LogError("Exception Message: {Message}", exc.Message);
                _logger.LogError("Exception StackTrace: {StackTrace}", exc.StackTrace);
                return Problem("An unexpected error occurred on our end during the login process.", statusCode: 500);
            }
        }

        // POST: api/Account/refreshtoken
        [HttpPost]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RefreshToken([FromBody] TokenDTO oldToken)
        {
            try
            {
                TokenDTO? newToken = await _authService.RefreshToken(oldToken);

                if (newToken == null)
                {
                    return Unauthorized();
                }

                return Ok(newToken);
            }
            catch (Exception exc)
            {
                _logger.LogError("Exception Message: {Message}", exc.Message);
                _logger.LogError("Exception StackTrace: {StackTrace}", exc.StackTrace);
                return Problem("An unexpected error occurred on our end during the 'refresh token' process.", statusCode: 500);
            }
        }
    }
}
