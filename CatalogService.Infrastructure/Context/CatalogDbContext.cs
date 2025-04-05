using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Context;

public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ✅ Explicit table name mapping
        modelBuilder.Entity<Category>().ToTable("category");
        modelBuilder.Entity<Product>().ToTable("product");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(c => c.Name)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.HasOne(c => c.ParentCategory)
                  .WithMany(c => c.SubCategories)
                  .HasForeignKey(c => c.ParentCategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(p => p.Price)
                  .HasColumnType("decimal(18,2)");

            entity.Property(p => p.Amount)
                  .IsRequired();

            entity.HasOne(p => p.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(p => p.CategoryId);
        });
    }
}
