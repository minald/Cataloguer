using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cataloguer.Models
{
    public class Album
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PictureLink { get; set; }

        public long Scrobbles { get; set; }

        public int Listeners { get; set; }

        public virtual Artist Artist { get; set; }

        public virtual List<Song> Songs { get; set; }

        public Album()
        {
            Songs = new List<Song>();
        }
    }
}