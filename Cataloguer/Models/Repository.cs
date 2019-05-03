using Cataloguer.Data;
using System.Collections.Generic;
using System.Linq;

namespace Cataloguer.Models
{
    public class Repository
    {
        private ApplicationDbContext Db { get; set; }

        public Repository() => Db = new ApplicationDbContext();

        public Repository(ApplicationDbContext context) => Db = context;

        public List<Artist> GetArtists() => Db.Artists.ToList();

        public List<Artist> GetArtistsByName(string name) => Db.Artists.Where(a => a.Name == name).ToList();

        public List<Album> GetAlbums() => Db.Albums.ToList();

        public List<Album> GetAlbumsByName(string name) => Db.Albums.Where(a => a.Name == name).ToList();

        public List<Track> GetTracks() => Db.Tracks.ToList();

        public List<Track> GetTracksByName(string name) => Db.Tracks.Where(a => a.Name == name).ToList();

        public Artist GetArtist(string name) => Db.Artists.First(a => a.Name == name);

        public Album GetAlbum(string albumName, string artistName) 
            => Db.Artists.First(a => a.Name == artistName).Albums.First(a => a.Name == albumName);
    }
}
