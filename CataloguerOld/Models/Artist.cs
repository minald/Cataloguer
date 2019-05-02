using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cataloguer.Models
{
    public class Artist
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Column]
        private string PictureLink { get; set; }

        [Column]
        private string Scrobbles { get; set; }

        [Column]
        private string Listeners { get; set; }

        [Column]
        private string ShortBiography { get; set; }

        [Column]
        private string FullBiography { get; set; }

        public virtual List<Album> Albums { get; set; }

        public virtual List<Track> Tracks { get; set; }

        public virtual List<string> Tags { get; set; }

        public Artist() { }

        public Artist(string name)
        {
            Name = name;
            Albums = new List<Album>();
            Tracks = new List<Track>();
            Tags = new List<string>();
        }

        public void SetPictureLink(string pictureLink)
        {
            string defaultPictureLink = "https://lastfm-img2.akamaized.net/i/u/avatar170s/2a96cbd8b46e442fc41c2b86b821562f";
            PictureLink = PictureLink == "" ? defaultPictureLink : pictureLink;
        }

        public string GetPictureLink()
        {
            return PictureLink;
        }

        public void SetScrobbles(string scrobbles)
        {
            Scrobbles = NormalizeNumber(scrobbles);
        }

        public string GetScrobbles()
        {
            return Scrobbles;
        }

        public void SetListeners(string listeners)
        {
            Listeners = NormalizeNumber(listeners);
        }

        public string GetListeners()
        {
            return Listeners;
        }

        private string NormalizeNumber(string number)
        {
            int digits = number.Length;
            if (digits <= 3) return number;
            else number = number.Insert(digits - 3, " ");
            if (digits <= 6) return number;
            else number = number.Insert(digits - 6, " ");
            if (digits <= 9) return number;
            else return number.Insert(digits - 9, " ");
        }

        public void SetShortBiography(string shortBiography)
        {
            ShortBiography = NormalizeBiography(shortBiography);
        }

        public string GetShortBiography()
        {
            return ShortBiography;
        }

        public void SetFullBiography(string fullBiography)
        {
            FullBiography = NormalizeBiography(fullBiography);
        }

        public string GetFullBiography()
        {
            return FullBiography;
        }

        private string NormalizeBiography(string non_normalizedBiography)
        {
            int indexOfUnnecessaryLink = non_normalizedBiography.IndexOf("<a href=");
            if(indexOfUnnecessaryLink != -1)
            {
                return non_normalizedBiography.Substring(0, indexOfUnnecessaryLink);
            }
                
            return non_normalizedBiography;
        }
    }
}
