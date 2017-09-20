using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        public Artist(string name, string pictureLink, string profileLink, 
            string scrobbles, string listeners, List<Album> albums, List<Track> tracks, List<string> tags)
        {
            Name = name;
            PictureLink = pictureLink;
            ProfileLink = profileLink;
            Scrobbles = scrobbles;
            Listeners = listeners;
            Albums = albums;
            Tracks = tracks;
            Tags = tags;
        }
    }
}
