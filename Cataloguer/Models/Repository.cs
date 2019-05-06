using System.Collections.Generic;
using System.Linq;
using Cataloguer.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public void InsertOrUpdate(ApplicationUser obj)
        {
            var existingItem = Db.Users.Find(obj.Id);
            if (existingItem == null)
            {
                Db.Add(obj);
            }
            else
            {
                Db.Entry(existingItem).CurrentValues.SetValues(obj);
            }

            Db.SaveChanges();
        }

        public void InsertOrUpdate(Artist obj)
        {
            var existingItem = Db.Set<Artist>().FirstOrDefault(x => x.Name == obj.Name);
            if (existingItem == null)
            {
                Db.Add(obj);
            }
            else if(!string.IsNullOrWhiteSpace(obj.Scrobbles))
            {
                existingItem.Scrobbles = obj.Scrobbles;
            }

            Db.SaveChanges();
        }

        public void InsertOrUpdate(Track obj)
        {
            obj.Artist = Db.Set<Artist>().FirstOrDefault(a => a.Name == obj.Artist.Name);
            var existingItem = Db.Set<Track>()
                .FirstOrDefault(x => x.Name == obj.Name && x.Artist.Id == obj.Artist.Id);
            if (existingItem == null)
            {
                Db.Add(obj);
            }
            else if(!string.IsNullOrWhiteSpace(obj.Scrobbles))
            {
                existingItem.Scrobbles = obj.Scrobbles;
            }

            Db.SaveChanges();
        }

        public SelectList GetCountries() => new SelectList(Db.Countries, "Id", "Name");

        public SelectList GetLanguages() => new SelectList(Db.Languages, "Id", "Name");

        public SelectList GetTemperaments() => new SelectList(Db.Temperaments, "Id", "Name");
    }
}
