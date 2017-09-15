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

        public long Scrobbles { get; set; }

        public int Listeners { get; set; }

        public virtual List<Album> Albums { get; set; }

        public Artist()
        {
            Albums = new List<Album>();
        }
    }
}