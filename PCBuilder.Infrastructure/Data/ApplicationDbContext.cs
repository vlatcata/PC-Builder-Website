using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PCBuilder.Infrastructure.Data.Identity;

namespace PCBuilder.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Computer> Computers { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Component> Components { get; set; }

        public DbSet<Specification> Specifications { get; set; }

        public DbSet<CartComponent> CartComponent { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CartComponent>()
                .HasKey(c => new { c.CartId, c.ComponentId });

            modelBuilder.Entity<CartComponent>()
                .HasOne(c => c.Cart)
                .WithMany(c => c.CartComponents)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartComponent>()
                .HasOne(c => c.Component)
                .WithMany(c => c.ComponentCarts)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Cart>()
            //    .HasOne(a => a.TotalPrice)
            //    .WithOne(x => x.User);
        }
    }
}