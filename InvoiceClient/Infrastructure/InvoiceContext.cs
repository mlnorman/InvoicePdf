using InvoiceClient.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceClient.Infrastructure
{
    public class InvoiceContext : DbContext
    {
        public InvoiceContext(DbContextOptions<InvoiceContext> options) : base(options) { }

        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // There is no reason to use tracking with this 
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // fluent configuration
            modelBuilder.Entity<Invoice>(entity =>
            {

                entity.ToTable("Invoices", "dbo");

                // making invoice # the identity, assuming uniqueness
                entity.Property(x => x.InvoiceId).ValueGeneratedOnAdd();

                entity.Property(x => x.CreatedDate)
                    .IsRequired()
                    .HasColumnType("datetime");

                entity.Property(x => x.InvoicePeriodStart)
                    .IsRequired()
                    .HasColumnType("date");

                entity.Property(x => x.InvoicePeriodEnd)
                    .IsRequired()
                    .HasColumnType("date");

                entity.Property(x => x.DueDate)
                    .IsRequired()
                    .HasColumnType("date");

                entity.Property(x => x.DocumentExists)
                    .IsRequired()
                    .HasColumnType("bit")
                    .HasDefaultValue(false);
            });

        }
    }
}
