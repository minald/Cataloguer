using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cataloguer.Models
{
    public class Album
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PictureLink { get; set; }

        public string ReleaseDate { get; set; }

        public string RunningLenght { get; set; }

        public string RunningTime { get; set; }

        public string Scrobbles { get; set; }

        public string Listeners { get; set; }

        public virtual Artist Artist { get; set; }

        public virtual List<Song> Songs { get; set; }

        public virtual List<string> Tags { get; set; }

        public Album()
        {
            Songs = new List<Song>();
        }

        public Album(string name, string pictureLink, string listeners)
        {
            Name = name;
            PictureLink = pictureLink;
            Listeners = listeners;
            Songs = new List<Song>();
        }

        public Album(string name, string pictureLink, string releaseDate, string runningLenght, string listeners)
        {
            Name = name;
            PictureLink = pictureLink;
            ReleaseDate = releaseDate;
            RunningLenght = runningLenght;
            Listeners = listeners;
            Songs = new List<Song>();
        }
    }
}
