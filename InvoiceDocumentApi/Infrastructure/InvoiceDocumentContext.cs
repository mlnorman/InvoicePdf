using InvoiceDocumentApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceDocumentApi.Infrastructure
{
    public class InvoiceDocumentContext : DbContext
    {
        public InvoiceDocumentContext() { }

        public InvoiceDocumentContext(DbContextOptions<InvoiceDocumentContext> options) : base(options) { }

        public virtual DbSet<InvoiceDocument> InvoiceDocuments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // There is no reason to use tracking with this 
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // fluent configuration
            modelBuilder.Entity<InvoiceDocument>(entity =>
            {
                entity.ToTable("InvoiceDocuments");

                // Making an assumption invoice numbers are unique
                entity.HasKey(x => x.Id).HasName("PK_InvoiceDocumentsId");

                entity.HasIndex(x => x.InvoiceId).IsUnique();

                entity.Property(x => x.CreatedDate)
                    .IsRequired()
                    .HasColumnType("datetime");

                entity.Property(x => x.PdfDocument)
                    .IsRequired()
                    .HasColumnType("varbinary(max)");

            });               
        }
    }
}
