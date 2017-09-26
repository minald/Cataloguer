using System;

namespace Cataloguer.Models
{
    public class Track
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //public string LinkToAudio { get; set; }

        public int Rank { get; set; }

        public string Duration { get; set; }

        public string Scrobbles { get; set; }

        public string Listeners { get; set; }

        public virtual Album Album { get; set; }

        public virtual Artist Artist { get; set; }

        public Track() {}

        public Track(String name)
        {
            Name = name;
        }

        public void SetDuration(string seconds)
        {
            int allSeconds = Convert.ToInt32(seconds);
            int newMinutes = allSeconds / 60;
            int newSeconds = allSeconds % 60;
            if(newSeconds < 10) Duration = newMinutes + " : 0" + newSeconds;
            else Duration = newMinutes + " : " + newSeconds;
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
    }
}