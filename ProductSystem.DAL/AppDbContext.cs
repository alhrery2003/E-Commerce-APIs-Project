using Microsoft.EntityFrameworkCore;
using ProductSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ProductSystem.DAL
{
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(b =>
            {
                b.HasKey(c => c.Id);
                b.Property(c => c.Name).IsRequired().HasMaxLength(200);
            });

            modelBuilder.Entity<Product>(b =>
            {
                b.HasKey(p => p.Id);
                b.Property(p => p.Name).IsRequired().HasMaxLength(200);
                b.Property(p => p.Price).HasColumnType("decimal(18,2)");
                b.HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Order>(b =>
            {
                b.HasKey(o => o.Id);
                b.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
                b.HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(b =>
            {
                b.HasKey(oi => oi.Id);
                b.Property(oi => oi.UnitPrice).HasColumnType("decimal(18,2)");
                b.HasOne(oi => oi.Order).WithMany(o => o.Items).HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(oi => oi.Product).WithMany().HasForeignKey(oi => oi.ProductId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Cart>(b =>
            {
                b.HasKey(c => c.Id);
                b.HasOne(c => c.User).WithOne(u => u.Cart).HasForeignKey<Cart>(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CartItem>(b =>
            {
                b.HasKey(ci => ci.Id);
                b.HasOne(ci => ci.Cart).WithMany(c => c.Items).HasForeignKey(ci => ci.CartId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(ci => ci.Product).WithMany().HasForeignKey(ci => ci.ProductId).OnDelete(DeleteBehavior.Cascade);
            });

            // Seeding
            // Roles
            var adminRoleId = "a3f2a1b1-2c1a-4b9e-9d2a-5f3e7a1b2c3d";
            var userRoleId = "b4e3b1c2-3d2b-5c0f-0e3b-6f4f8b2c3d4e";

            var roles = new List<IdentityRole>
            {
                new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "5aaeead7-639a-4c27-ac1a-96b6d27448da" },
                new IdentityRole { Id = userRoleId, Name = "User", NormalizedName = "USER", ConcurrencyStamp = "d6e382f6-3c22-4952-b88d-e6b8a21d1b82" }
            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);

            // Users
            var adminUserId = "c1a2b3c4-d5e6-f7a8-b9c0-d1e2f3a4b5c6";
            var adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin@example.com",
                NormalizedUserName = "ADMIN@EXAMPLE.COM",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                FullName = "System Admin",
                PasswordHash = "AQAAAAIAAYagAAAAEPGgs/PM35DiyEFMM0fGAMxAeqW3h0ZwUrOinB9RhyY4AwGiayjui3FBUCRYa8HNlA==", // Admin@123
                SecurityStamp = "4407460B620147F88EC89D5DEBA8BBA6",
                ConcurrencyStamp = "62704c79-903e-4acb-a2b8-7a46ea8d0330"
            };

            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = adminUserId
            });

            var _categories = new List<Category>()
            {
                new Category {Id = 1, Name = "Gaming" },
                new Category {Id = 2, Name = "Clothes" },
                new Category {Id = 3, Name = "Electronics"},
                new Category {Id = 4, Name = "Fun" }
            };

            var _products = new List<Product>()
            {
                new Product { Id = 1, Name = "Gaming Mouse", Price = 850, CategoryId = 1, ImageUrl = null },
                new Product { Id = 2, Name = "Mechanical Keyboard", Price = 2200, CategoryId = 1, ImageUrl = null },
                new Product { Id = 3, Name = "Gaming Headset", Price = 1500, CategoryId = 1, ImageUrl = null },

                new Product { Id = 4, Name = "Men T-Shirt", Price = 300, CategoryId = 2, ImageUrl = null },
                new Product { Id = 5, Name = "Women Jacket", Price = 1200, CategoryId = 2, ImageUrl = null },
                new Product { Id = 6, Name = "Sports Shoes", Price = 1800, CategoryId = 2, ImageUrl = null },

                new Product { Id = 7, Name = "Smartphone", Price = 15000, CategoryId = 3, ImageUrl = null },
                new Product { Id = 8, Name = "Laptop", Price = 35000, CategoryId = 3, ImageUrl = null },
                new Product { Id = 9, Name = "Bluetooth Speaker", Price = 950, CategoryId = 3, ImageUrl = null },

                new Product { Id = 10, Name = "UNO Cards", Price = 120, CategoryId = 4, ImageUrl = null },
                new Product { Id = 11, Name = "Puzzle Game", Price = 250, CategoryId = 4, ImageUrl = null },
                new Product { Id = 12, Name = "RC Car Toy", Price = 900, CategoryId = 4, ImageUrl = null },
            };

            modelBuilder.Entity<Category>().HasData(_categories);
            modelBuilder.Entity<Product>().HasData(_products);
        }
    }
}
