using System;
using System.Threading.Tasks;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Interfaces;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;

namespace AdvancedDevSample.Application.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;

        public AuthService(IUserRepository userRepository, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<string> RegisterAsync(LoginRequest request)
        {
            if (await _userRepository.ExistsByEmailAsync(request.Email))
            {
                throw new InvalidOperationException("Cet email est déjà utilisé.");
            }

            var user = new User
            {
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _userRepository.AddAsync(user);
            return "Compte créé avec succès !";
        }

        public async Task<string?> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return null;
            }

            return _jwtProvider.GenerateToken(user.Id, user.Email);
        }
    }
}
