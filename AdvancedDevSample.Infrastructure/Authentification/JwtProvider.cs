using System;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using AdvancedDevSample.Application.Interfaces; // Indispensable pour IJwtProvider

namespace AdvancedDevSample.Infrastructure.Authentification
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _options;

        public JwtProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public string GenerateToken(Guid userId, string email)
        {
            // 1. Définir les "Claims" (les infos stockées dans le token)
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // 2. Créer la clé de sécurité
            // Note : Assurez-vous que votre SecretKey dans appsettings.json fait au moins 32 caractères !
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 3. Créer le token
            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Le token expire dans 1 heure
                signingCredentials: creds
            );

            // 4. Ecrire le token en string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}