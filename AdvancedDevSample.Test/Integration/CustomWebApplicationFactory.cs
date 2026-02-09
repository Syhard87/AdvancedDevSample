using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing; // <--- C'EST CETTE LIGNE QUI MANQUAIT !
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AdvancedDevSample.Infrastructure.Persistence;
using System.Linq;

namespace AdvancedDevSample.Test.Integration
{
    // Cette classe crée un "faux serveur" pour vos tests
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // 1. On cherche la configuration de la vraie base de données (SQLite)
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                // 2. Si on la trouve, on la supprime (pour ne pas toucher au vrai fichier .db)
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // 3. On la remplace par une Base de Données en Mémoire (RAM)
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });
            });
        }
    }
}