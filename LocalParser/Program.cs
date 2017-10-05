using System;
using System.IO;
using Cataloguer.Models;

namespace LocalParser
{
    public class Program
    {
        public static MusicRepository database = new MusicRepository();

        public static string mainDirectory = @"D:\Music\";

        public static void Main()
        {
            SetDataDirectory();
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
            database.Save();
        }

        private static void SetDataDirectory()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relative = @"..\..\..\Cataloguer\App_Data\";
            string absolute = Path.GetFullPath(Path.Combine(baseDirectory, relative));
            AppDomain.CurrentDomain.SetData("DataDirectory", absolute);
        }

        private static void AddArtist(string artistDirectory)
        {
            string artistName = new DirectoryInfo(artistDirectory).Name;
            if (!database.ArtistExists(artistName))
            {
                Artist artist = new Artist(artistName);
                artist.SetPictureLink("");
                database.AddArtist(artist);
                database.Save();
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
            if (!database.AlbumExists(albumName, artistName))
            {
                Album album = new Album(albumName);
                album.SetPictureLink("");
                database.AddAlbumToArtist(album, artistName);
                database.Save();
            }
        }

        private static void AddTrackToAlbumOfArtist(string trackDirectory, string albumName, string artistName)
        {
            string trackName = Path.GetFileNameWithoutExtension(trackDirectory);
            if (!database.TrackExists(trackName, albumName, artistName))
            {
                Track track = new Track(trackName);
                track.LinkToAudio = trackDirectory;
                track.SetPictureLink("");
                database.AddTrackToAlbumOfArtist(track, albumName, artistName);
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
            if (!database.TrackExists(trackName, artistName))
            {
                Track track = new Track(trackName);
                track.LinkToAudio = trackDirectory;
                track.SetPictureLink("");
                database.AddTrackToArtist(track, artistName);
            }
        }
    }
}
