using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Models;

namespace UrlShortener.Api.Data
{
    public class UrlShortenerDbContext : DbContext
    {
        public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : base(options)
        {
            Accounts = Set<Account>();
        }

        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Account>()
                .HasIndex(x => x.Login)
                .IsUnique();

            modelBuilder.Entity<Account>().HasData(
                new Account { Id = 1, Login = "admin_paul_stone", Password = "Password123!", IsAdmin = true },
                new Account { Id = 2, Login = "user_paul_stone", Password = "Password123!", IsAdmin = false }
            );
        }
    }
}