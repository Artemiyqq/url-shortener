using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Models;

namespace UrlShortener.Api.Data
{
    public class UrlShortenerDbContext : DbContext
    {
        public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : base(options)
        {
            Accounts = Set<Account>();
            ShortenedUrls = Set<ShortenedUrl>();
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Account>()
                .HasIndex(x => x.Login)
                .IsUnique();

            modelBuilder.Entity<ShortenedUrl>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<ShortenedUrl>()
                .HasIndex(su => su.LongUrl)
                .IsUnique();
            
            modelBuilder.Entity<ShortenedUrl>()
                .HasOne(su => su.Account)
                .WithMany(a => a.ShortenedUrls)
                .HasForeignKey(su => su.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}