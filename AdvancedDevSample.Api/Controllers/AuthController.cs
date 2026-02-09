using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Interfaces;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net; // Indispensable pour la sécurité

namespace AdvancedDevSample.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtProvider _jwtProvider;
        private readonly ApplicationDbContext _context;

        public AuthController(IJwtProvider jwtProvider, ApplicationDbContext context)
        {
            _jwtProvider = jwtProvider;
            _context = context;
        }

        // --- C'EST CETTE PARTIE QUI VOUS MANQUE ---
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginRequest request)
        {
            // 1. On vérifie si l'email existe déjà
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest("Cet email est déjà utilisé.");
            }

            // 2. On crée l'utilisateur
            var user = new User
            {
                Email = request.Email,
                // 3. On HACHE le mot de passe (sécurité)
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Compte créé avec succès ! Vous pouvez vous connecter.");
        }
        // ------------------------------------------

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return Unauthorized("Email ou mot de passe incorrect.");
            }

            // On vérifie le Hash
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return Unauthorized("Email ou mot de passe incorrect.");
            }

            string token = _jwtProvider.GenerateToken(user.Id, user.Email);

            return Ok(new { token = token });
        }
    }
}