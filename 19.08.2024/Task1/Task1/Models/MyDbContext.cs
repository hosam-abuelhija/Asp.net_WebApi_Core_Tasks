using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Task1.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-SF9S359;Database=webapitask1;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            //entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            //entity.Property(e => e.CategoryImage)
            //    .HasMaxLength(5000)
            //    .IsUnicode(false);
            //entity.Property(e => e.CategoryName)
            //    .HasMaxLength(255)
            //    .IsUnicode(false);
            modelBuilder.Entity<Product>()
           .HasOne(p => p.Category)
           .WithMany(c => c.Products)
           .HasForeignKey(p => p.CategoryId);

            base.OnModelCreating(modelBuilder);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6ED6C153BED");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Description)
                .HasMaxLength(225)
                .IsUnicode(false);
            entity.Property(e => e.Price)
                .HasMaxLength(225)
                .IsUnicode(false);
            entity.Property(e => e.ProductImage).IsUnicode(false);
            entity.Property(e => e.ProductName)
                .HasMaxLength(225)
                .IsUnicode(false);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Products__Catego__398D8EEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
