using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cataloguer.Models
{
    public class Track
    {
        public int Id { get; set; }

        public int Rating { get; set; }

        public string Name { get; set; }

        public string Listeners { get; set; }

        public virtual Artist Artist { get; set; }

        public Track()
        {
        }

        public Track(int rating, string name, string listeners)
        {
            Rating = rating;
            Name = name;
            Listeners = listeners;
        }
    }
}