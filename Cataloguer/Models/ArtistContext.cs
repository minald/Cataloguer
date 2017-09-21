using System.Data.Entity;

namespace Cataloguer.Models
{
    public class ArtistContext : DbContext
    {
        public DbSet<Artist> Artists { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Track> Tracks { get; set; }
    }
}