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

        public AccountController(IAuthService authService)
        {
            _authService = authService;
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
                IEnumerable<IdentityError> errors = await _authService.RegisterUser(userDTO);
                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                return Ok();
            }
            catch (Exception exc)
            {
                Console.WriteLine();
                Console.WriteLine(exc.Message);
                Console.WriteLine();
                Console.WriteLine(exc.StackTrace);
                Console.WriteLine();
                return Problem("An unexpected error occurred on our end.");
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
                TokenDTO? tokenDTO = await _authService.Login(loginDTO);
                if (tokenDTO == null)
                {
                    return Unauthorized();
                }
                return Ok(tokenDTO);
            }
            catch (Exception exc)
            {
                Console.WriteLine();
                Console.WriteLine(exc.Message);
                Console.WriteLine();
                Console.WriteLine(exc.StackTrace);
                Console.WriteLine();
                return Problem("An unexpected error occurred on our end.");
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
            TokenDTO? newToken = await _authService.RefreshToken(oldToken);

            if (newToken == null)
            {
                return Unauthorized();
            }

            return Ok(newToken);
        }
    }
}
