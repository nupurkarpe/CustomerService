using CustomerService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CustomerService.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }     
        public DbSet<CustomerDetails> customerDetails { get; set; }
        public DbSet<DocType> docType { get; set; }
        public DbSet<Kyc> kyc { get; set; }
    }
}
