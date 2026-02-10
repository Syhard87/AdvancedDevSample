using System;
using System.Threading.Tasks;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Services; // Ensure this is using Services, not Interfaces for class injection
using Microsoft.AspNetCore.Mvc;

namespace AdvancedDevSample.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginRequest request)
        {
            try
            {
                var result = await _authService.RegisterAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.LoginAsync(request);

            if (token == null)
            {
                return Unauthorized("Email ou mot de passe incorrect.");
            }

            return Ok(new { token = token });
        }
    }
}