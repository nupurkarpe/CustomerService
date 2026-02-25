using CustomerService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CustomerService.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }     
        public DbSet<CustomerDetails> customerDetails { get; set; }
        public DbSet<DocType> docType { get; set; }
        public DbSet<Kyc> kyc { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configure one-to-many: CustomerDetails (1) -> Kyc (many)
            modelBuilder.Entity<CustomerDetails>()
                .HasMany(c => c.kyc)
                .WithOne(k => k.customerDetails)
                .HasForeignKey(k => k.customerId)
                .OnDelete(DeleteBehavior.Cascade);

            // ensure indexes are not unique (remove unique constraint created by previous model)
            modelBuilder.Entity<Kyc>()
                .HasIndex(k => k.customerId)
                .IsUnique(false);

            modelBuilder.Entity<Kyc>()
                .HasIndex(k => k.docTypeId)
                .IsUnique(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
