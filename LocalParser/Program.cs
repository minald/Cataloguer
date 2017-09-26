using System.IO;
using System.Collections.Generic;
using System.Linq;
using Cataloguer.Models;

namespace LocalParser
{
    public class Program
    {
        public static ArtistContext database = new ArtistContext();

        public static void Main()
        {
            string mainDirectoryName = @"C:\Users\a.minald\source\repos\Cataloguer\Cataloguer\Content\Music\";
            string[] directoriesWithArtists = Directory.GetDirectories(mainDirectoryName);
            foreach (string artistDirectory in directoriesWithArtists)
            {
                string artistName = artistDirectory.Substring(mainDirectoryName.Length);
                if (!ArtistExists(artistName))
                {
                    Artist artist = new Artist
                    {
                        Name = artistName,
                        Albums = new List<Album>(),
                        Tracks = new List<Track>()
                    };
                    database.Artists.Add(artist);
                    database.SaveChanges();
                }
                if (Directory.Exists(artistDirectory + @"\Albums"))
                {
                    string[] directoriesWithAlbums = Directory.GetDirectories(artistDirectory + @"\Albums");
                    foreach(string albumDirectory in directoriesWithAlbums)
                    {
                        int indexOfLastSlash = albumDirectory.LastIndexOf(@"\") + 1;
                        string albumName = albumDirectory.Substring(indexOfLastSlash);
                        if (!AlbumExists(albumName, artistName))
                        {
                            Album album = new Album
                            {
                                Name = albumName,
                                Tracks = new List<Track>()
                            };
                            database.Artists.Where(a => a.Name == artistName).First().Albums.Add(album);
                            database.SaveChanges();
                        }
                        string[] directoriesWithTracks = Directory.GetFiles(albumDirectory);
                        foreach (string trackDirectory in directoriesWithTracks)
                        {
                            indexOfLastSlash = trackDirectory.LastIndexOf(@"\") + 1;
                            string trackName = trackDirectory.Substring(indexOfLastSlash);
                            if (!TrackExists(trackName, albumName, artistName))
                            {
                                Track track = new Track(trackName);
                                track.Scrobbles = trackDirectory.Substring(67);
                                database.Artists.Where(a => a.Name == artistName).First().
                                    Albums.Where(a => a.Name == albumName).First().Tracks.Add(track);
                            }
                        }
                    }
                }   
                if (Directory.Exists(artistDirectory + @"\Tracks"))
                {
                    string[] directoriesWithTracks = Directory.GetFiles(artistDirectory + @"\Tracks");
                    foreach(string trackDirectory in directoriesWithTracks)
                    {
                        int indexOfLastSlash = trackDirectory.LastIndexOf(@"\") + 1;
                        string trackName = trackDirectory.Substring(indexOfLastSlash);
                        if (!TrackExists(trackName, artistName))
                        {
                            Track track = new Track(trackName);
                            track.Scrobbles = trackDirectory.Substring(67);
                            database.Artists.Where(a => a.Name == artistName).First().Tracks.Add(track);
                        }
                    }
                }
            }
            database.SaveChanges();
        }

        private static bool ArtistExists(string name)
        {
            IQueryable<Artist> artists = database.Artists;
            IQueryable<Artist> artistsWithSuitableName = artists.Where(a => a.Name == name);
            if (artistsWithSuitableName.Count() == 0)
                return false;
            return true;
        }

        private static bool AlbumExists(string albumName, string artistName)
        {
            IQueryable<Album> albums = database.Albums;
            IQueryable<Album> albumsWithSuitableName = albums.Where(a => a.Name == albumName).
                Where(a => a.Artist.Name == artistName);
            if (albumsWithSuitableName.Count() == 0)
                return false;
            return true;
        }

        private static bool TrackExists(string trackName, string artistName)
        {
            IQueryable<Track> tracks = database.Tracks;
            IQueryable<Track> tracksWithSuitableNameAndArtist = tracks.Where(t => t.Name == trackName).
                Where(t => t.Artist.Name == artistName);
            if (tracksWithSuitableNameAndArtist.Count() == 0)
                return false;
            return true;
        }

        private static bool TrackExists(string trackName, string albumName, string artistName)
        {
            IQueryable<Track> tracks = database.Tracks;
            IQueryable<Track> tracksWithSuitableNameAndAlbumAndArtist = tracks.Where(t => t.Name == trackName).
                Where(t => t.Album.Name == albumName).Where(t => t.Album.Artist.Name == artistName);
            if (tracksWithSuitableNameAndAlbumAndArtist.Count() == 0)
                return false;
            return true;
        }
    }
}
