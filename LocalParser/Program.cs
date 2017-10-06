using System;
using System.IO;
using Cataloguer.Models;
using System.Net;

namespace LocalParser
{
    public class Program
    {
        public static MusicRepository database = new MusicRepository();

        public static LastFMParser parser = new LastFMParser();

        public static WebClient client = new WebClient();

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
            string name = new DirectoryInfo(artistDirectory).Name;
            Artist artist = parser.GetArtist(name);
            string picturePath = artistDirectory + @"\" + name + ".png";
            if (!File.Exists(picturePath))
            {
                client.DownloadFile(artist.GetPictureLink(), picturePath);
            }
            SetArtistInfo(artist, picturePath);
        }

        private static void SetArtistInfo(Artist artist, string picturePath)
        {
            if (database.ArtistExists(artist.Name))
            {
                SetExistingArtistInfo(artist, picturePath);
            }
            else
            {
                SetNewArtistInfo(artist, picturePath);
            }
            database.Save();
        }

        private static void SetExistingArtistInfo(Artist artist, string picturePath)
        {
            Artist newArtist = database.GetArtist(artist.Name);
            newArtist.SetPictureLink(picturePath);
            newArtist.SetScrobbles(artist.GetScrobbles().Replace(" ", ""));
            newArtist.SetListeners(artist.GetListeners().Replace(" ", ""));
            newArtist.SetShortBiography(artist.GetShortBiography());
            database.UpdateArtist(newArtist); 
        }

        private static void SetNewArtistInfo(Artist artist, string picturePath)
        {
            Artist newArtist = new Artist(artist.Name);
            newArtist.SetPictureLink(picturePath);
            newArtist.SetScrobbles(artist.GetScrobbles().Replace(" ", ""));
            newArtist.SetListeners(artist.GetListeners().Replace(" ", ""));
            newArtist.SetShortBiography(artist.GetShortBiography());
            database.AddArtist(newArtist);
        }  

        private static void ParseAlbums(string artistDirectory)
        {
            string artistName = new DirectoryInfo(artistDirectory).Name;
            foreach (string albumDirectory in Directory.GetDirectories(artistDirectory + @"\Albums"))
            {
                string albumName = new DirectoryInfo(albumDirectory).Name;
                //AddAlbumToArtist(albumName, artistName);
                foreach (string trackDirectory in Directory.GetFiles(albumDirectory))
                {
                    //AddTrackToAlbumOfArtist(trackDirectory, albumName, artistName);
                }
            }
        }

        private static void AddAlbumToArtist(string albumName, string artistName)
        {
            Album album = parser.GetAlbum(albumName, artistName);
            string picturePath = mainDirectory + artistName + @"\Albums\" + albumName + ".png";
            if (!File.Exists(picturePath))
            {
                client.DownloadFile(album.GetPictureLink(), picturePath);
            }
            SetAlbumInfo(album, picturePath);
        }

        private static void SetAlbumInfo(Album album, string picturePath)
        {
            if (database.AlbumExists(album.Name, album.Artist.Name))
            {
                SetExistingAlbumInfo(album, picturePath);
            }
            else
            {
                SetNewAlbumInfo(album, picturePath);
            }
            database.Save();
        }

        private static void SetExistingAlbumInfo(Album album, string picturePath)
        {
            Album newAlbum = database.GetAlbum(album.Name, album.Artist.Name);
            album.SetPictureLink(picturePath);
            album.SetScrobbles(album.GetScrobbles().Replace(" ", ""));
            album.SetListeners(album.GetListeners().Replace(" ", ""));
            database.UpdateAlbum(album);
        }

        private static void SetNewAlbumInfo(Album album, string picturePath)
        {
            Album newAlbum = new Album(album.Name);
            album.SetPictureLink(picturePath);
            album.SetScrobbles(album.GetScrobbles().Replace(" ", ""));
            album.SetListeners(album.GetListeners().Replace(" ", ""));
            database.AddAlbumToArtist(album, album.Artist.Name);
        }

        private static void AddTrackToAlbumOfArtist(string trackDirectory, string albumName, string artistName)
        {
            string trackName = Path.GetFileNameWithoutExtension(trackDirectory);
            if (!database.TrackExists(trackName, albumName, artistName))
            {
                Track track = new Track(trackName);
                track.LinkToAudio = trackDirectory;
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
                database.AddTrackToArtist(track, artistName);
            }
        }
    }
}
