using System;
using System.Collections.Generic;

namespace Cataloguer.Models
{
    public class Track
    {
        public int Id { get; set; }

        public string Name { get; set; }

        private string PictureLink { get; set; }

        public string LinkToAudio { get; set; }

        public int Rank { get; set; }

        private string Duration { get; set; }

        private string Scrobbles { get; set; }

        private string Listeners { get; set; }

        private string Info { get; set; }

        public virtual List<string> Tags { get; set; }

        public virtual Album Album { get; set; }

        public virtual Artist Artist { get; set; }

        public Track(String name)
        {
            Name = name;
            Tags = new List<string>();
        }

        public void SetPictureLink(string pictureLink)
        {
            string defaultPictureLink = "https://lastfm-img2.akamaized.net/i/u/174s/4128a6eb29f94943c9d206c08e625904";
            if (pictureLink != "")
                PictureLink = pictureLink;
            else
                PictureLink = defaultPictureLink;
        }

        public string GetPictureLink()
        {
            return PictureLink;
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
            if(newSeconds < 10) Duration = newMinutes + " : 0" + newSeconds;
            else Duration = newMinutes + " : " + newSeconds;
        }

        public string GetDuration()
        {
            return Duration;
        }

        public void SetScrobbles(string scrobbles)
        {
            Scrobbles = NormalizeNumber(scrobbles);
        }

        public string GetScrobbles()
        {
            return Scrobbles;
        }

        public void SetListeners(string listeners)
        {
            Listeners = NormalizeNumber(listeners);
        }

        public string GetListeners()
        {
            return Listeners;
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

        public string GetInfo()
        {
            return Info;
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