using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;
using System.Drawing;
using System.Security.Claims;

namespace ProductAPI.Data
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        Product p = new Product();
        public DbSet<Product> Products { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductInventory> ProductInventories { get; set; }
        public DbSet<ProductModel> ProductModels { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set; }
        public DbSet<ProductProductPhoto> ProductProductPhotos { get; set; }
        public DbSet<ProductSubcategory> productSubcategories { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductInventory>()
                .HasKey(p => new { p.ProductID, p.LocationID });

            modelBuilder.Entity<ProductProductPhoto>()
                .HasKey(p => new { p.ProductID, p.ProductPhotoID });

            modelBuilder.Entity<UnitMeasure>()
                .HasKey(p => new { p.UnitMeasureCode});

        }
    }
}
