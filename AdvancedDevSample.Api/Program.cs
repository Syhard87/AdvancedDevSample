using AdvancedDevSample.Application.Interfaces;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Domain.Interfaces;
// On garde votre dossier avec la faute d'orthographe (Authentification), c'est important !
using AdvancedDevSample.Infrastructure.Authentification;
using AdvancedDevSample.Infrastructure.Persistence;
// IMPORTANT : On réactive l'accès au dossier Repositories
using AdvancedDevSample.Infrastructure.Repositories;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURATION BDD (SQLite) ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// --- 2. CONFIGURATION SERVICES ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger avec support du Token JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mon API", Version = "v1" });

    // Définition de la sécurité (Le cadenas dans Swagger)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Entrez le token JWT comme ceci : Bearer votre_token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
    {
        new OpenApiSecurityScheme{ Reference = new OpenApiReference{ Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
        new string[]{}
    }});
});

// --- 3. CONFIGURATION JWT ---
var jwtSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtOptions>(jwtSection);

var secretKey = jwtSection.GetValue<string>("SecretKey");
var issuer = jwtSection.GetValue<string>("Issuer");
var audience = jwtSection.GetValue<string>("Audience");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// --- 4. INJECTION DES DEPENDANCES ---

// A. Repository SQLite
builder.Services.AddScoped<IProductRepository, SqliteProductRepository>();

// B. Services Applicatifs
// B. Services Applicatifs
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// C. Service d'Authentification (JWT)
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

// D. Repositories
builder.Services.AddScoped<IOrderRepository, SqliteOrderRepository>();

var app = builder.Build();

// --- 5. PIPELINE HTTP ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Ordre important : Auth -> Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// 👇 AJOUT INDISPENSABLE POUR LES TESTS D'INTÉGRATION 👇
// Cela rend la classe "Program" visible pour WebApplicationFactory dans le projet de Test
public partial class Program { }