using System;

namespace Cataloguer.Models
{
    public class Track : CommonObject
    {
        public string LinkToAudio { get; set; }
        public int Rank { get; set; }
        public string Duration { get; set; }
        public string Info { get; set; }

        public virtual Album Album { get; set; }
        public virtual Artist Artist { get; set; }

        public Track() { }

        public Track(String name) => Name = name;

        public override string GetDefaultPictureLink()
            => "https://lastfm-img2.akamaized.net/i/u/174s/4128a6eb29f94943c9d206c08e625904";

        public void SetDurationInMilliseconds(string milliseconds)
        {
            string seconds = milliseconds.Length <= 3 ? "0" : milliseconds.Substring(0, milliseconds.Length - 3);
            SetDuration(seconds);
        }

        public void SetDuration(string seconds)
        {
            int allSeconds = Convert.ToInt32(seconds);
            int newMinutes = allSeconds / 60;
            int newSeconds = allSeconds % 60;
            Duration = newMinutes + " : " + (newSeconds < 10 ? "0" : "") + newSeconds; 
        }

        public void SetInfo(string info) => Info = NormalizeInfo(info);

        private string NormalizeInfo(string nonNormalizedInfo)
        {
            int indexOfUnnecessaryLink = nonNormalizedInfo.IndexOf("<a href=");
            return indexOfUnnecessaryLink == -1 ? 
                nonNormalizedInfo : nonNormalizedInfo.Substring(0, indexOfUnnecessaryLink);
        }
    }
}
