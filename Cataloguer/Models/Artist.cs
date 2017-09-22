using System.Collections.Generic;

namespace Cataloguer.Models
{
    public class Artist
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ProfileLink { get; set; }

        public string PictureLink { get; set; }

        public string Scrobbles { get; set; }

        public string Listeners { get; set; }

        public string ShortBiography { get; set; }

        public string FullBiography { get; set; }

        public virtual List<Album> Albums { get; set; }

        public virtual List<Track> Tracks { get; set; }

        public virtual List<string> Tags { get; set; }

        public class Builder
        {
            public string _name { get; set; }

            public string _profileLink { get; set; }

            public string _pictureLink { get; set; }

            public string _scrobbles { get; set; }

            public string _listeners { get; set; }

            public string _shortBiography { get; set; }

            public string _fullBiography { get; set; }

            public List<Album> _albums { get; set; }

            public List<Track> _tracks { get; set; }

            public List<string> _tags { get; set; }

            public Builder(string name, string profileLink)
            {
                _name = name;
                _profileLink = profileLink;
            }

            public Builder PictureLink(string pictureLink)
            {
                _pictureLink = pictureLink;
                return this;
            }

            public Builder Scrobbles(string scrobbles)
            {
                _scrobbles = scrobbles;
                return this;
            }

            public Builder Listeners(string listeners)
            {
                _listeners = listeners;
                return this;
            }

            public Builder ShortBiography(string shortBiography)
            {
                _shortBiography = shortBiography;
                return this;
            }

            public Builder FullBiography(string fullBiography)
            {
                _fullBiography = fullBiography;
                return this;
            }

            public Builder Albums(List<Album> albums)
            {
                _albums = albums;
                return this;
            }

            public Builder Tracks(List<Track> tracks)
            {
                _tracks = tracks;
                return this;
            }

            public Builder Tags(List<string> tags)
            {
                _tags = tags;
                return this;
            }

            public Artist Build()
            {
                return new Artist(this);
            }
        }

        public Artist(string name)
        {
            Name = name;
        }
 
        public Artist(Builder builder)
        {
            Name = builder._name;
            ProfileLink = builder._profileLink;
            PictureLink = builder._pictureLink;
            Scrobbles = builder._scrobbles;
            Listeners = builder._listeners;
            ShortBiography = builder._shortBiography;
            FullBiography = builder._fullBiography;
            Albums = builder._albums;
            Tracks = builder._tracks;
            Tags = builder._tags;
        }
    }
}
