using System;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace AdvancedDevSample.Test.Integration
{
    // On utilise notre Factory pour lancer l'API
    public class ProductIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ProductIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task FullFlow_CreateProduct_Should_Work_When_Authenticated()
        {
            // 1. REGISTER (Inscription)
            var email = $"testuser_{Guid.NewGuid()}@test.com";
            var authRequest = new { Email = email, Password = "Password123!" };

            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", authRequest);
            registerResponse.EnsureSuccessStatusCode();

            // 2. LOGIN (Connexion)
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", authRequest);
            var loginResult = await loginResponse.Content.ReadFromJsonAsync<TokenResponse>();

            var token = loginResult?.Token;
            token.Should().NotBeNullOrEmpty();

            // 3. AJOUT DU TOKEN (Autorisation)
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 4. CRÉATION DE PRODUIT (Test final)
            var newProduct = new { Name = "Integration Product", Price = 99.99m };
            var productResponse = await _client.PostAsJsonAsync("/api/products", newProduct);

            // 5. VÉRIFICATION
            // CORRECTIF ICI : On attend Created (201) et non OK (200)
            productResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task GetProducts_Should_Fail_When_No_Token()
        {
            // On essaie d'accéder SANS token
            // Comme on a sécurisé le contrôleur, cela doit renvoyer 401 Unauthorized
            var response = await _client.GetAsync("/api/products");
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }

    public class TokenResponse
    {
        public string Token { get; set; }
    }
}