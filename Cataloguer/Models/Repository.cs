using Cataloguer.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Cataloguer.Models
{
    public class Repository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public List<Artist> GetArtists() => db.Artists.ToList();

        public List<Artist> GetArtistsByName(string name) => db.Artists.Where(a => a.Name == name).ToList();

        public List<Album> GetAlbums() => db.Albums.ToList();

        public List<Album> GetAlbumsByName(string name) => db.Albums.Where(a => a.Name == name).ToList();

        public List<Track> GetTracks() => db.Tracks.ToList();

        public List<Track> GetTracksByName(string name) => db.Tracks.Where(a => a.Name == name).ToList();

        public Artist GetArtist(string name) => db.Artists.First(a => a.Name == name);

        public void UpdateArtist(Artist artist)
        {
            db.Entry(artist).State = EntityState.Modified;
        }

        public Album GetAlbum(string albumName, string artistName)
        {
            return db.Artists.First(a => a.Name == artistName).Albums.First(a => a.Name == albumName);
        }

        public void UpdateAlbum(Album album)
        {
            db.Entry(album).State = EntityState.Modified;
        }

        public void AddArtist(Artist artist)
        {
            db.Artists.Add(artist);
        }

        public void AddAlbum(Album album)
        {
            db.Albums.Add(album);
        }

        public void AddAlbumToArtist(Album album, string artistName)
        {
            db.Artists.First(a => a.Name == artistName).Albums.Add(album);
        }

        public void AddTrackToAlbumOfArtist(Track track, string albumName, string artistName)
        {
            db.Artists.First(a => a.Name == artistName).Albums.First(a => a.Name == albumName).Tracks.Add(track);
        }

        public void AddTrackToArtist(Track track, string artistName)
        {
            db.Artists.First(a => a.Name == artistName).Tracks.Add(track);
        }

        public bool ArtistExists(string name) => db.Artists.Any(a => a.Name == name);

        public bool AlbumExists(string albumName, string artistName) =>
            db.Albums.Any(a => a.Name == albumName && a.Artist.Name == artistName);

        public bool TrackExists(string trackName, string artistName) =>
            db.Tracks.Any(t => t.Name == trackName && t.Artist.Name == artistName);

        public bool TrackExists(string trackName, string albumName, string artistName) =>
            db.Tracks.Any(t => t.Name == trackName && t.Album.Name == albumName && t.Album.Artist.Name == artistName);

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
