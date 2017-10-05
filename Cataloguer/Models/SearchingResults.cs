using System.Collections.Generic;

namespace Cataloguer.Models
{
    public class SearchingResults
    {
        public List<Artist> LastFMArtists { get; set; }

        public List<Album> LastFMAlbums { get; set; }

        public List<Track> LastFMTracks { get; set; }

        public List<Artist> LocalArtists { get; set; }

        public List<Album> LocalAlbums { get; set; }

        public List<Track> LocalTracks { get; set; }

        public SearchingResults() { }
    }
}