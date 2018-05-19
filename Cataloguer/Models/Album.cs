using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cataloguer.Models
{
    public class Album
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Column]
        private string PictureLink { get; set; }

        [Column]
        private string Scrobbles { get; set; }

        [Column]
        private string Listeners { get; set; }

        public virtual List<Track> Tracks { get; set; }

        public virtual List<string> Tags { get; set; }

        public virtual Artist Artist { get; set; }

        public Album() { }

        public Album(string name)
        {
            Name = name;
            Tracks = new List<Track>();
            Tags = new List<string>();
        }

        public void SetPictureLink(string pictureLink)
        {
            string defaultPictureLink = "https://lastfm-img2.akamaized.net/i/u/174s/c6f59c1e5e7240a4c0d427abd71f3dbb";
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
    }
}
