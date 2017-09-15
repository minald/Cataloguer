using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cataloguer.Models
{
    public class Song
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public long Scrobbles { get; set; }

        public int Listeners { get; set; }

        public virtual Album Album{ get; set; }
    }
}