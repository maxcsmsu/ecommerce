using ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.Data;

public class eCommerceDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public eCommerceDbContext(DbContextOptions<eCommerceDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // configure  

        // Map entities to tables  
        modelBuilder.Entity<Product>().ToTable("Products");
        // Configure Primary Keys 
        modelBuilder.Entity<Product>().HasKey(u => u.Id).HasName("PK_Products");
        // Configure indexes  
        modelBuilder.Entity<Product>().HasIndex(p => p.Name).IsUnique().HasDatabaseName("Idx_Name");


        // Configure columns  
        modelBuilder.Entity<Product>().Property(ug => ug.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
        modelBuilder.Entity<Product>().Property(ug => ug.Name).HasColumnType("nvarchar(100)").IsRequired();
        modelBuilder.Entity<Product>().Property(ug => ug.Name).HasColumnType("nvarchar(100)").IsRequired();

        // Mock data to entity
        modelBuilder.Entity<Product>().HasData(new Product[] {
                new Product { Id = 1, Name = "สเปรย์ดับเพลิง", Description = ""},
                new Product { Id = 2, Name ="ยาแนวรอยต่อ", Description = ""},
                new Product { Id = 3, Name = "ชั้นรถเข็นเล็ก", Description = ""},
                new Product { Id = 4, Name = "ฉนวนกันความร้อน", Description = ""},
                new Product { Id = 5, Name = "เทปพันท่อ", Description = ""},
                new Product { Id = 6, Name = "ซิลิโคน", Description = ""},
                new Product { Id = 7, Name = "สายยาง", Description = ""},
            });
    }
}

