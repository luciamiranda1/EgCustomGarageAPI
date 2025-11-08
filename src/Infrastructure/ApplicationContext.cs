using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        //Constructor
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder) //genera las tablas y relaciones.
        {
            // User.Role como string en DB
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion(new EnumToStringConverter<UserRole>());

            // Product -> Category (N:1)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category) //un producto tiene una categoría
                .WithMany(c => c.Products) //una categoría tiene muchos productos
                .HasForeignKey(p => p.CategoryId) //CategoryId es la clave foránea en la tabla Products.
                .OnDelete(DeleteBehavior.Restrict); //si intentás borrar una categoría que tiene productos, te lo bloquea

            // Order -> Product (N:1)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product) //cada pedido tiene un producto
                .WithMany() //el producto no tiene una lista de pedidos 
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order -> User (N:1) (Client)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Client)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}