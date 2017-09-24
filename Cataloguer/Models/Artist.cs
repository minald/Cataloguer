using System.Collections.Generic;

namespace Cataloguer.Models
{
    public class Artist
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PictureLink { get; set; }

        public string Scrobbles { get; set; }

        public string Listeners { get; set; }

        public string ShortBiography { get; set; }

        public string FullBiography { get; set; }

        public virtual List<Album> Albums { get; set; }

        public virtual List<Track> Tracks { get; set; }

        public virtual List<string> Tags { get; set; }

        public Artist() {}

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

        public void SetShortBiography(string shortBiography)
        {
            ShortBiography = NormalizeBiography(shortBiography);
        }

        public void SetFullBiography(string fullBiography)
        {
            FullBiography = NormalizeBiography(fullBiography);
        }

        private string NormalizeBiography(string non_normalizedBiography)
        {
            int indexOfUnnecessaryLink = non_normalizedBiography.IndexOf("<a href=");
            string normalizedBiography = non_normalizedBiography.Substring(0, indexOfUnnecessaryLink);
            return normalizedBiography;
        }
    }
}
