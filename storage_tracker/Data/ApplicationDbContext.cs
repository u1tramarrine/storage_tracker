using Microsoft.EntityFrameworkCore;
using storage_tracker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace storage_tracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Box> Boxes { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasColumnName("id")  
                      .HasDefaultValueSql("uuid_generate_v4()");
                entity.Property(e => e.Name)
                      .HasColumnName("name")
                      .IsRequired()
                      .HasMaxLength(255);
                entity.Property(e => e.Description)
                      .HasColumnName("description")
                      .HasColumnType("text");

                entity.HasIndex(e => e.Name).IsUnique();
            });

            // Box configuration
            modelBuilder.Entity<Box>(entity =>
            {
                entity.ToTable("boxes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .HasDefaultValueSql("uuid_generate_v4()");
                entity.Property(e => e.Name)
                      .HasColumnName("name")
                      .IsRequired()
                      .HasMaxLength(255);
                entity.Property(e => e.Location)
                      .HasColumnName("location")
                      .HasMaxLength(255);
                entity.Property(e => e.Description)
                      .HasColumnName("description")
                      .HasColumnType("text");
                entity.Property(e => e.PhotoUrl)
                      .HasColumnName("photo_url")
                      .HasMaxLength(255);
                entity.Property(e => e.CategoryId)
                      .HasColumnName("category_id");

                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Boxes)
                      .HasForeignKey(e => e.CategoryId)
                      .HasConstraintName("boxes_category_id_fkey")
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Item configuration
            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("items");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .HasDefaultValueSql("uuid_generate_v4()");
                entity.Property(e => e.Name)
                      .HasColumnName("name")
                      .IsRequired()
                      .HasMaxLength(255);
                entity.Property(e => e.Description)
                      .HasColumnName("description")
                      .HasColumnType("text");
                entity.Property(e => e.Quantity)
                      .HasColumnName("quantity")
                      .IsRequired();
                entity.Property(e => e.Price)
                      .HasColumnName("price")
                      .HasPrecision(6, 2);
                entity.Property(e => e.PhotoUrl)
                      .HasColumnName("photo_url")
                      .HasMaxLength(255);
                entity.Property(e => e.BoxId)
                      .HasColumnName("box_id");
                entity.Property(e => e.CategoryId)
                      .HasColumnName("category_id");
                entity.Property(e => e.Notes)
                      .HasColumnName("notes")
                      .HasMaxLength(255);
                entity.Property(e => e.CreatedAt)
                      .HasColumnName("created_at")
                      .IsRequired()
                      .HasDefaultValueSql("CURRENT_DATE");

                entity.HasOne(e => e.Box)
                      .WithMany(b => b.Items)
                      .HasForeignKey(e => e.BoxId)
                      .HasConstraintName("items_box_id_fkey")
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Items)
                      .HasForeignKey(e => e.CategoryId)
                      .HasConstraintName("items_category_id_fkey")
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "Host=localhost;Database=storagetracker;Username=postgres;Password=ваш_пароль";
                optionsBuilder.UseNpgsql(connectionString);
            }
        }
    }
}
