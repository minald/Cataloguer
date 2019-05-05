using System.Collections.Generic;

namespace Cataloguer.Models
{
    public class SearchingResults
    {
        public List<Artist> Artists { get; set; }

        public List<Album> Albums { get; set; }

        public List<Track> Tracks { get; set; }

        public SearchingResults(List<Artist> artists, List<Album> albums, List<Track> tracks)
        {
            Artists = artists;
            Albums = albums;
            Tracks = tracks;
        }
    }
}
