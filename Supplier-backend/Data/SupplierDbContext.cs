using Microsoft.EntityFrameworkCore;
using Supplier_backend.Models;

namespace Supplier_backend.Data
{
    public class SupplierDbContext : DbContext
    {
        public SupplierDbContext(DbContextOptions<SupplierDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<QuoteItem> QuoteItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<RFQ> RFQs { get; set; }         
        public DbSet<RFQItem> RFQItems { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define primary keys
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<Product>().HasKey(p => p.ProductId);
            modelBuilder.Entity<Quote>().HasKey(q => q.QuoteId);
            modelBuilder.Entity<QuoteItem>().HasKey(qi => qi.QuoteItemId);
            modelBuilder.Entity<Order>().HasKey(o => o.OrderId);
            modelBuilder.Entity<OrderItem>().HasKey(oi => oi.OrderItemId);
            modelBuilder.Entity<Client>().HasKey(c => c.ClientId);
            modelBuilder.Entity<Inventory>().HasKey(i => i.InventoryId);
            modelBuilder.Entity<RFQ>().HasKey(r => r.RFQId);            
            modelBuilder.Entity<RFQItem>().HasKey(ri => ri.RFQItemId);

            // Ensure RFQId is not auto-generated
            modelBuilder.Entity<RFQ>()
                .Property(pr => pr.RFQId)
                .ValueGeneratedNever();

            // Ensure OrderId is not auto-generated
            modelBuilder.Entity<Order>()
                .Property(pr => pr.OrderId)
                .ValueGeneratedNever();

            modelBuilder.Entity<QuoteItem>()
            .Property(q => q.QuotedPrice)
            .HasColumnType("decimal(18, 2)");

            // Convert enum to string in the database
            modelBuilder.Entity<RFQ>()
                .Property(r => r.Status)
                .HasConversion<string>(); 

            // Define relationships
            modelBuilder.Entity<Product>()
                .HasMany(p => p.QuoteItems)
                .WithOne(qi => qi.Product)
                .HasForeignKey(qi => qi.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Client)
                .HasForeignKey(o => o.ClientId);

            modelBuilder.Entity<Quote>()
                .HasMany(q => q.QuoteItems)
                .WithOne(qi => qi.Quote)
                .HasForeignKey(qi => qi.QuoteId);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Product)
                .WithOne(p => p.Inventory)
                .HasForeignKey<Inventory>(i => i.ProductId);

            // Define RFQ and RFQItem relationships
            modelBuilder.Entity<RFQ>()
                .HasMany(r => r.RFQItems)
                .WithOne(ri => ri.RFQ)
                .HasForeignKey(ri => ri.RFQId);

            modelBuilder.Entity<RFQ>()
                .HasMany(r => r.Quotes)
                .WithOne(q => q.RFQ)
                .HasForeignKey(q => q.RFQId);

            modelBuilder.Entity<RFQItem>()
                .HasOne(ri => ri.Product)
                .WithMany(p => p.RFQItems)
                .HasForeignKey(ri => ri.ProductId);

            modelBuilder.Entity<Quote>()
                .HasOne(q => q.Client)
                .WithMany(c => c.Quotes)
                .HasForeignKey(q => q.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
