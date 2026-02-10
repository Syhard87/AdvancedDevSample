using System;
using System.Threading.Tasks;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Interfaces;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace AdvancedDevSample.Test.UnitTests
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IJwtProvider> _jwtProviderMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _jwtProviderMock = new Mock<IJwtProvider>();
            _authService = new AuthService(_userRepositoryMock.Object, _jwtProviderMock.Object);
        }

        [Fact]
        public async Task LoginAsync_Should_ReturnToken_When_CredentialsValid()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password123";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            
            var user = new User { Email = email, PasswordHash = hashedPassword };
            // Set Id via reflection if needed, or assume default/random
            
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(email))
                .ReturnsAsync(user);
                
            _jwtProviderMock.Setup(j => j.GenerateToken(It.IsAny<Guid>(), email))
                .Returns("valid-token");

            var request = new LoginRequest { Email = email, Password = password };

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            result.Should().Be("valid-token");
        }

        [Fact]
        public async Task LoginAsync_Should_ReturnNull_When_UserNotFound()
        {
            // Arrange
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);
            
            var request = new LoginRequest { Email = "unknown", Password = "pwd" };

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task LoginAsync_Should_ReturnNull_When_PasswordInvalid()
        {
            // Arrange
            var user = new User { Email = "test", PasswordHash = BCrypt.Net.BCrypt.HashPassword("valid") };
            _userRepositoryMock.Setup(r => r.GetByEmailAsync("test")).ReturnsAsync(user);

            var request = new LoginRequest { Email = "test", Password = "wrong" };

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            result.Should().BeNull();
        }
    }
}
