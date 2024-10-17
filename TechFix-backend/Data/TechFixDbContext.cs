using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TechFix_backend.Models;

namespace TechFix_backend.Data
{
    public class TechFixDbContext : DbContext
    {
        public TechFixDbContext(DbContextOptions<TechFixDbContext> options) : base(options) { }

        // DbSets for each entity
        public DbSet<User> Users { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierQuote> SupplierQuotes { get; set; }
        public DbSet<SupplierQuoteItem> SupplierQuoteItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<ProductRef> ProductRefs { get; set; }
        public DbSet<RFQ> RFQs { get; set; }
        public DbSet<RFQItem> RFQItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define primary keys for each entity
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<Supplier>().HasKey(s => s.SupplierId);
            modelBuilder.Entity<SupplierQuote>().HasKey(q => q.SupplierQuoteId);
            modelBuilder.Entity<SupplierQuoteItem>().HasKey(qi => qi.SupplierQuoteItemId);
            modelBuilder.Entity<Order>().HasKey(o => o.OrderId);
            modelBuilder.Entity<OrderItem>().HasKey(oi => oi.OrderItemId);
            modelBuilder.Entity<Inventory>().HasKey(i => i.InventoryId);
            modelBuilder.Entity<ProductRef>().HasKey(pr => pr.ProductId);
            modelBuilder.Entity<RFQ>().HasKey(r => r.RFQId);
            modelBuilder.Entity<RFQItem>().HasKey(ri => ri.RFQItemId);

            // Convert enum to string in the database
            modelBuilder.Entity<RFQ>()
                .Property(r => r.Status)
                .HasConversion<string>();
            
            // Set ProductId in ProductRef to not auto-generate
            modelBuilder.Entity<ProductRef>()
                .Property(pr => pr.ProductId)
                .ValueGeneratedNever(); // Ensure ProductId is not auto-generated

            // Define relationships for SupplierQuote
            modelBuilder.Entity<SupplierQuote>()
                .HasOne(q => q.Supplier)
                .WithMany(s => s.SupplierQuotes)
                .HasForeignKey(q => q.SupplierId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<SupplierQuote>()
                .HasMany(q => q.SupplierQuoteItems)
                .WithOne(i => i.SupplierQuote)
                .HasForeignKey(i => i.SupplierQuoteId);

            // Define relationships for Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Supplier)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.SupplierId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Quote)
                .WithMany()
                .HasForeignKey(o => o.QuoteId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            // Define relationships for Inventory
            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Supplier)
                .WithMany(s => s.Inventories)
                .HasForeignKey(i => i.SupplierId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            // Configure relationships for ProductRef
            modelBuilder.Entity<ProductRef>()
                .HasOne(pr => pr.Supplier)
                .WithMany(s => s.ProductRefs)
                .HasForeignKey(pr => pr.SupplierId);

            // RFQ Configuration
            modelBuilder.Entity<RFQ>()
                .HasMany(r => r.RFQItems)
                .WithOne(i => i.RFQ)
                .HasForeignKey(i => i.RFQId);

            modelBuilder.Entity<RFQ>()
                .HasMany(r => r.SupplierQuotes)
                .WithOne(q => q.RFQ)
                .HasForeignKey(q => q.RFQId);

            // RFQItem Configuration
            modelBuilder.Entity<RFQItem>()
                .HasOne(i => i.RFQ)
                .WithMany(r => r.RFQItems)
                .HasForeignKey(i => i.RFQId);

            // RFQItem should not map to SupplierQuoteItem; this should map only within RFQ
            modelBuilder.Entity<RFQItem>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId);

            // SupplierQuoteItem Configuration
            modelBuilder.Entity<SupplierQuoteItem>()
                .HasOne(i => i.SupplierQuote)
                .WithMany(q => q.SupplierQuoteItems)
                .HasForeignKey(i => i.SupplierQuoteId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes for SupplierQuote

            modelBuilder.Entity<SupplierQuoteItem>()
                .HasOne(i => i.Product)
                .WithMany() // Depending on Product relationship design
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes for Product
        }
    }
}
