using Abstraction.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Position>()
                .HasOne(x => x.Portfolio)
                .WithMany(x => x.Positions).HasForeignKey(x=>x.PortfolioId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Portfolio>()
                .HasMany(x => x.Positions).WithOne(x=>x.Portfolio)
                .HasForeignKey(x => x.PortfolioId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Position>()
                .HasOne(x => x.Stock)
                .WithMany(x => x.Positions).HasForeignKey(x=>x.StockId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Stock>()
                .HasMany(x => x.Positions).WithOne(x=>x.Stock)
                .HasForeignKey(x => x.StockId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Position> Positions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Stock> Stocks { get; set; }

    }
}