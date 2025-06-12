using Microsoft.AspNetCore.Mvc;
using Server.Model.Account.Dto.Request;
using Server.Model.Account.Dto.Response;
using Server.Model.Token.Dto;
using Server.Service.Interface;

namespace Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest req)
        {
            var success = await _authService.RegisterAsync(req.PlayerId, req.Password);
            if (!success)
                return Conflict(new RegisterResponse
                {
                    PlayerId = req.PlayerId,
                    Message = "Id already exists"
                });

            return Ok(new RegisterResponse
            {
                PlayerId = req.PlayerId,
                Message = "Register Success"
            });
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest req)
        {
            var token = await _authService.LoginAsync(req.PlayerId, req.Password);
            if (token == null)
                return Unauthorized(new LoginResponse
                {
                    PlayerId = req.PlayerId,
                    Token = null,
                    Message = "Invalid credentials"
                });

            return Ok(new LoginResponse
            {
                PlayerId = req.PlayerId,
                Token = token,
                Message = "Login successful"
            });
        }
    }
}
