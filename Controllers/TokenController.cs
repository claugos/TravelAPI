using Microsoft.AspNetCore.Mvc;
using System.Net;
using TravelAPI.Services;

namespace TravelAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<ReservationController> _logger;
        private readonly IConfiguration _config;

        public TokenController(IConfiguration config, ILogger<ReservationController> logger)
        {
            _config = config;
            _logger = logger;
        }

        [HttpPost("GenerateToken")]
        public IActionResult GenerateToken()
        {
            try
            {
                JWTService jWT = new(_config);
                string tokenString = jWT.CreateToken();
                return Ok(new { token = tokenString });
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred generating the token:  {ErrorMessage}", e.Message);
                return BadRequest(new
                {
                    code = HttpStatusCode.BadRequest,
                    message = "An error occurred generating the token."
                });
            }
        }
    }
}
