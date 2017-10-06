using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;

namespace Cataloguer.Models
{
    public class MusicContext : DbContext
    {
        public DbSet<Artist> Artists { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Track> Tracks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new NonPublicColumnAttributeConvention());
        }
    }

    public sealed class NonPublicColumnAttributeConvention : Convention
    {
        public NonPublicColumnAttributeConvention()
        {
            Types().Having(NonPublicProperties).Configure((config, properties) =>
            {
                foreach (PropertyInfo prop in properties)
                {
                    config.Property(prop);
                }
            });
        }

        private IEnumerable<PropertyInfo> NonPublicProperties(Type type)
        {
            var matchingProperties = type.
                GetProperties(BindingFlags.SetProperty | BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(propInfo => propInfo.GetCustomAttributes(typeof(ColumnAttribute), true).Length > 0).ToArray();
            return matchingProperties.Length == 0 ? null : matchingProperties;
        }
    }

    public class MusicRepository
    {
        private MusicContext database = new MusicContext();

        public List<Artist> GetArtists()
        {
            return database.Artists.ToList();
        }

        public List<Artist> GetArtistsByName(string name)
        {
            return database.Artists.Where(a => a.Name == name).ToList();
        }

        public List<Album> GetAlbums()
        {
            return database.Albums.ToList();
        }

        public List<Album> GetAlbumsByName(string name)
        {
            return database.Albums.Where(a => a.Name == name).ToList();
        }

        public List<Track> GetTracks()
        {
            return database.Tracks.ToList();
        }

        public List<Track> GetTracksByName(string name)
        {
            return database.Tracks.Where(a => a.Name == name).ToList();
        }

        public Artist GetArtist(string name)
        {
            return database.Artists.First(a => a.Name == name);
        }

        public Album GetAlbum(string albumName, string artistName)
        {
            return database.Artists.First(a => a.Name == artistName).Albums.First(a => a.Name == albumName);
        }

        public void AddArtist(Artist artist)
        {
            database.Artists.Add(artist);
        }

        public void AddAlbum(Album album)
        {
            database.Albums.Add(album);
        }

        public void AddAlbumToArtist(Album album, string artistName)
        {
            database.Artists.First(a => a.Name == artistName).Albums.Add(album);
        }

        public void AddTrackToAlbumOfArtist(Track track, string albumName, string artistName)
        {
            database.Artists.First(a => a.Name == artistName).Albums.First(a => a.Name == albumName).Tracks.Add(track);
        }

        public void AddTrackToArtist(Track track, string artistName)
        {
            database.Artists.First(a => a.Name == artistName).Tracks.Add(track);
        }

        public bool ArtistExists(string name)
        {
            if (database.Artists.Any(a => a.Name == name))
                return true;
            return false;
        }

        public bool AlbumExists(string albumName, string artistName)
        {
            if (database.Albums.Any(a => a.Name == albumName && a.Artist.Name == artistName))
                return true;
            return false;
        }

        public bool TrackExists(string trackName, string artistName)
        {
            if (database.Tracks.Any(t => t.Name == trackName && t.Artist.Name == artistName))
                return true;
            return false;
        }

        public bool TrackExists(string trackName, string albumName, string artistName)
        {
            if (database.Tracks.Any(t => t.Name == trackName && t.Album.Name == albumName && t.Album.Artist.Name == artistName))
                return true;
            return false;
        }

        public void Save()
        {
            database.SaveChanges();
        }
    }
}