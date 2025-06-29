using DiscountCodeGenerator.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCodeGenerator.Db
{
    public class DiscountCodeContext : DbContext
    {
        public DbSet<DiscountCode> DiscountCodes { get; set; }

        public DiscountCodeContext(DbContextOptions<DiscountCodeContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiscountCode>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Code)
                      .IsRequired()
                      .HasMaxLength(8);

                entity.HasIndex(e => e.Code)
                      .IsUnique();

                entity.Property(e => e.IsUsed)
                      .IsRequired();

                entity.Property(e => e.CreatedAt)
                      .IsRequired();
            });
        }
    }
}
