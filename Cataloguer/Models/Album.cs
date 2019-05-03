using System.Collections.Generic;

namespace Cataloguer.Models
{
    public class Album : CommonObject
    {
        public virtual List<Track> Tracks { get; set; } = new List<Track>();
        public virtual Artist Artist { get; set; }

        public Album() { }

        public Album(string name) => Name = name;

        public override string GetDefaultPictureLink()
            => "https://lastfm-img2.akamaized.net/i/u/174s/c6f59c1e5e7240a4c0d427abd71f3dbb";
    }
}
