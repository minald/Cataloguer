﻿using System.Collections.Generic;

namespace Cataloguer.Models
{
    public class Album
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PictureLink { get; set; }

        public string ReleaseDate { get; set; }

        public string Duration { get; set; }

        public string Scrobbles { get; set; }

        public string Listeners { get; set; }

        public virtual List<Track> Tracks { get; set; }

        public virtual List<string> Tags { get; set; }

        public virtual Artist Artist { get; set; }

        public Album() {}

        public Album(string name)
        {
            Name = name;
        }

        public void SetPictureLink(string pictureLink)
        {
            string defaultPictureLink = "https://lastfm-img2.akamaized.net/i/u/174s/c6f59c1e5e7240a4c0d427abd71f3dbb";
            if (pictureLink != "")
                PictureLink = pictureLink;
            else
                PictureLink = defaultPictureLink;
        }

        public void SetScrobbles(string scrobbles)
        {
            Scrobbles = NormalizeNumber(scrobbles);
        }

        public void SetListeners(string listeners)
        {
            Listeners = NormalizeNumber(listeners);
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
