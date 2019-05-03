using System.Collections.Generic;

namespace Cataloguer.Models
{
    public class Artist : CommonObject
    {
        public string ShortBiography { get; set; }
        public string FullBiography { get; set; }

        public virtual List<Album> Albums { get; set; } = new List<Album>();
        public virtual List<Track> Tracks { get; set; } = new List<Track>();

        public Artist() { }

        public Artist(string name) => Name = name;

        public override string GetDefaultPictureLink() 
            => "https://lastfm-img2.akamaized.net/i/u/avatar170s/2a96cbd8b46e442fc41c2b86b821562f";

        public void SetShortBiography(string shortBiography) => ShortBiography = NormalizeBiography(shortBiography);

        public void SetFullBiography(string fullBiography) => FullBiography = NormalizeBiography(fullBiography);

        private string NormalizeBiography(string nonNormalizedBiography)
        {
            int indexOfUnnecessaryLink = nonNormalizedBiography.IndexOf("<a href=");
            return indexOfUnnecessaryLink == -1 ?
                nonNormalizedBiography : nonNormalizedBiography.Substring(0, indexOfUnnecessaryLink);
        }
    }
}
