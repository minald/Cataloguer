using System.IO;
using System.Linq;
using Cataloguer.Models;
using System;

namespace LocalParser
{
    public class Program
    {
        public static ArtistContext database = new ArtistContext();

        public static string mainDirectory = @"D:\Music\";

        public static void Main()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relative = @"..\..\..\Cataloguer\App_Data\";
            string absolute = Path.GetFullPath(Path.Combine(baseDirectory, relative));
            AppDomain.CurrentDomain.SetData("DataDirectory", absolute);

            foreach (string artistDirectory in Directory.GetDirectories(mainDirectory))
            {
                AddArtist(artistDirectory);
                if (Directory.Exists(artistDirectory + @"\Albums"))
                {
                    ParseAlbums(artistDirectory);
                }   
                if (Directory.Exists(artistDirectory + @"\Tracks"))
                {
                    ParseTracks(artistDirectory);
                }
            }
            database.SaveChanges();
        }

        private static void AddArtist(string artistDirectory)
        {
            string artistName = new DirectoryInfo(artistDirectory).Name;
            if (!ArtistExists(artistName))
            {
                Artist artist = new Artist(artistName);
                artist.SetPictureLink("");
                database.Artists.Add(artist);
                database.SaveChanges();
            }
        }

        private static void ParseAlbums(string artistDirectory)
        {
            string artistName = new DirectoryInfo(artistDirectory).Name;
            foreach (string albumDirectory in Directory.GetDirectories(artistDirectory + @"\Albums"))
            {
                string albumName = new DirectoryInfo(albumDirectory).Name;
                AddAlbumToArtist(albumName, artistName);
                foreach (string trackDirectory in Directory.GetFiles(albumDirectory))
                {
                    AddTrackToAlbumOfArtist(trackDirectory, albumName, artistName);
                }
            }
        }

        private static void AddAlbumToArtist(string albumName, string artistName)
        {
            if (!AlbumExists(albumName, artistName))
            {
                Album album = new Album(albumName);
                album.SetPictureLink("");
                database.Artists.First(a => a.Name == artistName).Albums.Add(album);
                database.SaveChanges();
            }
        }

        private static void AddTrackToAlbumOfArtist(string trackDirectory, string albumName, string artistName)
        {
            string trackName = Path.GetFileNameWithoutExtension(trackDirectory);
            if (!TrackExists(trackName, albumName, artistName))
            {
                Track track = new Track(trackName);
                track.LinkToAudio = trackDirectory;
                database.Artists.First(a => a.Name == artistName).
                    Albums.First(a => a.Name == albumName).Tracks.Add(track);
            }
        }

        private static void ParseTracks(string artistDirectory)
        {
            string artistName = new DirectoryInfo(artistDirectory).Name;
            string[] directoriesWithTracks = Directory.GetFiles(artistDirectory + @"\Tracks");
            foreach (string trackDirectory in directoriesWithTracks)
            {
                AddTrackToArtist(trackDirectory, artistName);
            }
        }

        private static void AddTrackToArtist(string trackDirectory, string artistName)
        {
            string trackName = Path.GetFileNameWithoutExtension(trackDirectory);
            if (!TrackExists(trackName, artistName))
            {
                Track track = new Track(trackName);
                track.LinkToAudio = trackDirectory;
                database.Artists.First(a => a.Name == artistName).Tracks.Add(track);
            }
        }

        private static bool ArtistExists(string name)
        {
            if (database.Artists.Any(a => a.Name == name))
                return true;
            return false;
        }

        private static bool AlbumExists(string albumName, string artistName)
        {
            if (database.Albums.Any(a => a.Name == albumName && a.Artist.Name == artistName))
                return true;
            return false;
        }

        private static bool TrackExists(string trackName, string artistName)
        {
            if (database.Tracks.Any(t => t.Name == trackName && t.Artist.Name == artistName))
                return true;
            return false;
        }

        private static bool TrackExists(string trackName, string albumName, string artistName)
        {
            if (database.Tracks.Any(t => t.Name == trackName && t.Album.Name == albumName && t.Album.Artist.Name == artistName))
                return true;
            return false;
        }
    }
}
