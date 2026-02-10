using Microsoft.EntityFrameworkCore;
using AdvancedDevSample.Domain.Entities;

namespace AdvancedDevSample.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Syntaxe "Fluent" simplifiée
            // On configure l'entité Product
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id); // Clé primaire

            // On configure le Value Object Price
            modelBuilder.Entity<Product>()
                .OwnsOne(p => p.Price)          // Le produit possède un Prix
                .Property(m => m.Value)         // La propriété "Value" du Prix...
                .HasColumnName("Price")         // ...ira dans la colonne "Price"
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}