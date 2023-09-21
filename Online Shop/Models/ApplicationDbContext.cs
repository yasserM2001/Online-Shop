using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Online_Shop.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<ProductCart> ProductCarts { get; set; }
        public DbSet<CartStatus> CartStatuses { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Category
            builder.Entity<Category>()
                .Property(c => c.Name).IsRequired();
            builder.Entity<Category>()
                .Property(c => c.InsertionDate)
                .HasDefaultValueSql("GETDATE()");
            builder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(c => c.Category)
                .OnDelete(DeleteBehavior.Restrict);
            // Product
            builder.Entity<Product>()
                .Property(p => p.Name).IsRequired();
            builder.Entity<Product>()
                .Property(p => p.InsertionDate)
                .HasDefaultValueSql("GETDATE()");
            // Cart

            // ProductCart
            builder.Entity<ProductCart>()
                .HasOne(c => c.Cart)
                .WithMany(c => c.Products)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<ProductCart>()
                .HasOne(c => c.Product)
                .WithMany(c => c.Carts)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<ProductCart>()
                .Property(x => x.InsertionDate)
                .HasDefaultValueSql("GETDATE()");
            // CartStatus
            //builder.Entity<CartStatus>()
            //               .HasNoKey();
        }
    }
}
