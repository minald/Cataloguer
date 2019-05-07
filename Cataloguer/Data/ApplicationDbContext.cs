using Cataloguer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cataloguer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Artist> Artists { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Track> Tracks { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Temperament> Temperaments { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public ApplicationDbContext() : base() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().Property(c => c.CountryId).HasDefaultValue(1);
            modelBuilder.Entity<ApplicationUser>().Property(c => c.SecondLanguageId).HasDefaultValue(1);
            modelBuilder.Entity<ApplicationUser>().Property(c => c.TemperamentId).HasDefaultValue(1);
        }
    }
}
