using System.Collections.Generic;

namespace Cataloguer.Models
{
    public class Album
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PageLink { get; set; }

        public string PictureLink { get; set; }

        public string ReleaseDate { get; set; }

        public string RunningLenght { get; set; }

        public string RunningTime { get; set; }

        public string Scrobbles { get; set; }

        public string Listeners { get; set; }

        public virtual List<Track> Tracks { get; set; }

        public virtual List<string> Tags { get; set; }

        //For binding in databases
        public virtual Artist Artist { get; set; }

        public Album() {}
    }
}
