using System;
using System.Collections.Generic;

namespace Cataloguer.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureLink { get; set; }
        public string LinkToAudio { get; set; }
        public int Rank { get; set; }
        public string Duration { get; set; }
        public string Scrobbles { get; set; }
        public string Listeners { get; set; }
        public string Info { get; set; }

        public virtual List<Tag> Tags { get; set; }
        public virtual Album Album { get; set; }
        public virtual Artist Artist { get; set; }

        public Track() { }

        public Track(String name)
        {
            Name = name;
            Tags = new List<Tag>();
        }

        public void SetPictureLink(string pictureLink)
        {
            string defaultPictureLink = "https://lastfm-img2.akamaized.net/i/u/174s/4128a6eb29f94943c9d206c08e625904";
            PictureLink = PictureLink == "" ? defaultPictureLink : pictureLink;
        }

        public void SetDurationInMilliseconds(string milliseconds)
        {
            if(milliseconds.Length < 4)
            {
                Duration = "0 : 00";
            }
            else
            {
                SetDuration(milliseconds.Substring(0, milliseconds.Length - 3));
            }
        }

        public void SetDuration(string seconds)
        {
            int allSeconds = Convert.ToInt32(seconds);
            int newMinutes = allSeconds / 60;
            int newSeconds = allSeconds % 60;
            Duration = newMinutes + " : " + (newSeconds < 10 ? "0" : "") + newSeconds; 
        }

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

        public void SetInfo(string info)
        {
            Info = NormalizeInfo(info);
        }

        private string NormalizeInfo(string non_normalizedInfo)
        {
            int indexOfUnnecessaryLink = non_normalizedInfo.IndexOf("<a href=");
            if (indexOfUnnecessaryLink != -1)
                return non_normalizedInfo.Substring(0, indexOfUnnecessaryLink);
            return non_normalizedInfo;
        }
    }
}
