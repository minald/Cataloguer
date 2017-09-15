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

        public string Scrobbles { get; set; }

        public string Listeners { get; set; }

        public virtual List<Album> Albums { get; set; }

        public Artist()
        {
            Albums = new List<Album>();
        }

        public Artist(string name, string pictureLink, string scrobbles, string listeners)
        {
            Name = name;
            PictureLink = pictureLink;
            Scrobbles = scrobbles;
            Listeners = listeners;
            Albums = new List<Album>();
        }
    }
}