using System.Collections.Generic;

namespace Cataloguer.Models
{
    public class Artist
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PictureLink { get; set; }

        public string ProfileLink { get; set; }

        public string Scrobbles { get; set; }

        public string Listeners { get; set; }

        public string ShortBiography { get; set; }

        public string FullBiography { get; set; }

        public virtual List<Album> Albums { get; set; }

        public virtual List<Track> Tracks { get; set; }

        public virtual List<string> Tags { get; set; }

        public Artist()
        {
            Albums = new List<Album>();
            Tracks = new List<Track>();
            Tags = new List<string>();
        }

        public Artist(string name, string pictureLink, string profileLink)
        {
            Name = name;
            PictureLink = pictureLink;
            ProfileLink = profileLink;
            Albums = new List<Album>();
            Tracks = new List<Track>();
            Tags = new List<string>();
        }

        public Artist(string name, string pictureLink, string profileLink, string fullBiography)
        {
            Name = name;
            PictureLink = pictureLink;
            ProfileLink = profileLink;
            FullBiography = fullBiography;
            Albums = new List<Album>();
            Tracks = new List<Track>();
            Tags = new List<string>();
        }

        public Artist(string name, string pictureLink, List<Album> albums)
        {
            Name = name;
            PictureLink = pictureLink;
            Albums = albums;
            Tracks = new List<Track>();
            Tags = new List<string>();
        }

        public Artist(string name, string pictureLink, List<Track> tracks)
        {
            Name = name;
            PictureLink = pictureLink;
            Albums = new List<Album>();
            Tracks = tracks;
            Tags = new List<string>();
        } 

        public Artist(string name, string pictureLink, string profileLink, string scrobbles,
             string listeners, string shortBiography, List<Album> albums, List<Track> tracks, List<string> tags)
        {
            Name = name;
            PictureLink = pictureLink;
            ProfileLink = profileLink;
            Scrobbles = scrobbles;
            Listeners = listeners;
            ShortBiography = shortBiography;
            Albums = albums;
            Tracks = tracks;
            Tags = tags;
        }
    }
}
