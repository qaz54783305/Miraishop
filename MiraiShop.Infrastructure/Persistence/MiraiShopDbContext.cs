using Microsoft.EntityFrameworkCore;
using MiraiShop.Domain.Entities;

namespace MiraiShop.Infrastructure.Persistence;

public class MiraiShopDbContext : DbContext
{
    public MiraiShopDbContext(DbContextOptions<MiraiShopDbContext> options)
        : base(options) { }

    public DbSet<Member> Members => Set<Member>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Cart> Carts => Set<Cart>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart", table =>
                table.HasCheckConstraint("CK_Cart_Quantity_Positive", "[Quantity] > 0"));

            // 同一會員的同一商品只保留一筆購物車明細。
            entity.HasKey(c => new { c.MemberId, c.ProductId });

            entity.Property(c => c.MemberId).ValueGeneratedNever();
            entity.Property(c => c.ProductId).ValueGeneratedNever();
            entity.Property(c => c.Quantity).IsRequired();
            entity.Property(c => c.UpdatedAt).IsRequired();

            entity.HasOne<Member>()
                  .WithMany()
                  .HasForeignKey(c => c.MemberId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Product>()
                  .WithMany()
                  .HasForeignKey(c => c.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(c => c.ProductId);
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.ToTable("Member");

            entity.HasKey(m => m.Id);
            entity.Property(m => m.Id)
                  .ValueGeneratedNever();

            entity.Property(m => m.Name)
                  .IsRequired();

            entity.Property(m => m.Email)
                  .HasMaxLength(256)
                  .IsRequired();

            entity.HasIndex(m => m.Email)
                  .IsUnique();

            entity.Property(m => m.PasswordHash)
                  .IsRequired();

            entity.Property(m => m.PasswordSalt);

            entity.Property(m => m.MailingAddress)
                  .IsRequired();

            entity.Property(m => m.ResidentialAddress);

            entity.Property(m => m.CreatedAt)
                  .IsRequired();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.HasKey(o => o.Id);
            entity.Property(o => o.Id)
                  .ValueGeneratedNever();

            entity.Property(o => o.MemberId)
                  .IsRequired();

            entity.HasIndex(o => o.MemberId);

            entity.Property(o => o.Status)
                  .IsRequired();

            entity.Property(o => o.TotalAmount)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(o => o.CreatedAt)
                  .IsRequired();

            entity.HasOne<Member>()
                  .WithMany()
                  .HasForeignKey(o => o.MemberId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id)
                  .ValueGeneratedNever();

            entity.Property(c => c.CategoryCode)
                  .IsRequired();

            entity.HasIndex(c => c.CategoryCode)
                  .IsUnique();

            entity.Property(c => c.CategoryName)
                  .IsRequired();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id)
                  .ValueGeneratedNever();

            entity.Property(p => p.Name)
                  .IsRequired();

            entity.Property(p => p.Price)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(p => p.Stock)
                  .IsRequired();

            entity.Property(p => p.CreatedAt)
                  .IsRequired();

            entity.Ignore(p => p.CategoryCode);

            entity.HasIndex(p => p.CategoryId);

            entity.HasOne<Category>()
                  .WithMany()
                  .HasForeignKey(p => p.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
