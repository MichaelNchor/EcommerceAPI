using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EcommerceAPI.Models
{
    public partial class EcommerceAPIContext : DbContext
    {
        public EcommerceAPIContext()
        {
        }

        public EcommerceAPIContext(DbContextOptions<EcommerceAPIContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<Product> Product { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-K5527T2\\SQLEXPRESS;Database=EcommerceAPI;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => e.CartId)
                    .HasName("PK2")
                    .IsClustered(false);

                entity.Property(e => e.CartId).HasColumnName("CartID");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("PK1")
                    .IsClustered(false);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CartId).HasColumnName("CartID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ProductName).HasMaxLength(400);

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.CartId)
                    .HasConstraintName("RefCart4");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
